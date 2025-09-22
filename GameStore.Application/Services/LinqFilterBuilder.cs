using System.Linq.Expressions;
using System.Reflection;
using GameStore.Shared.Filters;

namespace GameStore.Application.Services;

/// <summary>
/// Costruisce espressioni LINQ a partire da un FilterGroup condiviso.
/// </summary>
public static class LinqFilterBuilder
{
    public static IQueryable<T> ApplyFilters<T>(IQueryable<T> source, FilterGroup? root)
    {
        if (root == null) return source;
        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        Expression? body = BuildGroupExpression<T>(parameter, root);
        if (body == null) return source;
        Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
        return source.Where(lambda);
    }

    private static Expression? BuildGroupExpression<T>(ParameterExpression parameter, FilterGroup group)
    {
        List<Expression> expressions = new();

        foreach (FilterCondition condition in group.Conditions)
        {
            Expression? expr = BuildConditionExpression<T>(parameter, condition);
            if (expr != null) expressions.Add(expr);
        }

        foreach (FilterGroup sub in group.Groups)
        {
            Expression? subExpr = BuildGroupExpression<T>(parameter, sub);
            if (subExpr != null) expressions.Add(subExpr);
        }

        if (expressions.Count == 0) return null;

        return group.Logic == LogicalOperator.And
            ? expressions.Aggregate(Expression.AndAlso)
            : expressions.Aggregate(Expression.OrElse);
    }

    private static Expression? BuildConditionExpression<T>(ParameterExpression parameter, FilterCondition condition)
    {
        if (string.IsNullOrWhiteSpace(condition.Field)) return null;

        MemberExpression member = Expression.PropertyOrField(parameter, condition.Field);
        Type memberType = Nullable.GetUnderlyingType(member.Type) ?? member.Type;

        // Handle null checks
        if (condition.Operator == ComparisonOperator.IsNull)
        {
            return Expression.Equal(member, Expression.Constant(null, member.Type));
        }
        if (condition.Operator == ComparisonOperator.IsNotNull)
        {
            return Expression.NotEqual(member, Expression.Constant(null, member.Type));
        }

        // Convert values
        object? value = ConvertTo(condition.Value, memberType);
        object? value2 = ConvertTo(condition.Value2, memberType);
        ConstantExpression right = Expression.Constant(value, memberType);

        Expression memberNonNull = member;
        if (member.Type != memberType)
        {
            memberNonNull = Expression.Convert(member, memberType);
        }

        switch (condition.Operator)
        {
            case ComparisonOperator.Equals:
                return BuildEquals(memberNonNull, right, memberType, caseInsensitive: condition.FieldType == FieldType.String);
            case ComparisonOperator.NotEquals:
                return Expression.Not(BuildEquals(memberNonNull, right, memberType, caseInsensitive: condition.FieldType == FieldType.String));
            case ComparisonOperator.GreaterThan:
                return Expression.GreaterThan(memberNonNull, right);
            case ComparisonOperator.GreaterThanOrEqual:
                return Expression.GreaterThanOrEqual(memberNonNull, right);
            case ComparisonOperator.LessThan:
                return Expression.LessThan(memberNonNull, right);
            case ComparisonOperator.LessThanOrEqual:
                return Expression.LessThanOrEqual(memberNonNull, right);
            case ComparisonOperator.Contains:
                return BuildStringCall(member, "Contains", value);
            case ComparisonOperator.StartsWith:
                return BuildStringCall(member, "StartsWith", value);
            case ComparisonOperator.EndsWith:
                return BuildStringCall(member, "EndsWith", value);
            case ComparisonOperator.In:
                return BuildIn(memberNonNull, condition, memberType);
            case ComparisonOperator.Between:
                if (value == null || value2 == null) return null;
                ConstantExpression low = Expression.Constant(value, memberType);
                ConstantExpression high = Expression.Constant(value2, memberType);
                return Expression.AndAlso(
                    Expression.GreaterThanOrEqual(memberNonNull, low),
                    Expression.LessThanOrEqual(memberNonNull, high));
            default:
                return null;
        }
    }

    private static Expression BuildEquals(Expression left, ConstantExpression right, Type memberType, bool caseInsensitive)
    {
        if (caseInsensitive && memberType == typeof(string))
        {
            MethodInfo equalsIgnore = typeof(string).GetMethod(
                nameof(string.Equals),
                new[] { typeof(string), typeof(string), typeof(StringComparison) })!;
            return Expression.Call(
                equalsIgnore,
                Expression.Convert(left, typeof(string)),
                Expression.Convert(right, typeof(string)),
                Expression.Constant(StringComparison.OrdinalIgnoreCase));
        }
        return Expression.Equal(left, right);
    }

    private static Expression BuildStringCall(Expression member, string methodName, object? value)
    {
        // member != null && member.Method(value, OrdinalIgnoreCase)
        MethodInfo? method = typeof(string).GetMethod(methodName, new[] { typeof(string), typeof(StringComparison) });
        if (method != null)
        {
            return Expression.AndAlso(
                Expression.NotEqual(member, Expression.Constant(null, member.Type)),
                Expression.Call(
                    member,
                    method,
                    Expression.Constant(Convert.ToString(value) ?? string.Empty, typeof(string)),
                    Expression.Constant(StringComparison.OrdinalIgnoreCase))
            );
        }

        // Fallback to culture-sensitive single-arg if two-arg not found
        MethodInfo methodOne = typeof(string).GetMethod(methodName, new[] { typeof(string) })!;
        return Expression.AndAlso(
            Expression.NotEqual(member, Expression.Constant(null, member.Type)),
            Expression.Call(
                Expression.Call(member, nameof(string.ToUpperInvariant), Type.EmptyTypes),
                methodOne,
                Expression.Call(
                    Expression.Constant(Convert.ToString(value) ?? string.Empty, typeof(string)),
                    nameof(string.ToUpperInvariant), Type.EmptyTypes))
        );
    }

    private static Expression BuildIn(Expression member, FilterCondition condition, Type memberType)
    {
        if (condition.Value is IEnumerable<object> enumerableObj)
        {
            var list = enumerableObj.Select(v => ConvertTo(v, memberType)).ToList();
            var array = Array.CreateInstance(memberType, list.Count);
            for (int i = 0; i < list.Count; i++) array.SetValue(list[i], i);
            ConstantExpression arrConst = Expression.Constant(array, array.GetType());
            MethodInfo contains = typeof(Enumerable).GetMethods()
                .First(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
                .MakeGenericMethod(memberType);
            return Expression.Call(contains, arrConst, member);
        }
        if (condition.Value is string csv)
        {
            var parts = csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                           .Select(s => ConvertTo(s, memberType))
                           .ToList();
            var array = Array.CreateInstance(memberType, parts.Count);
            for (int i = 0; i < parts.Count; i++) array.SetValue(parts[i], i);
            ConstantExpression arrConst = Expression.Constant(array, array.GetType());
            MethodInfo contains = typeof(Enumerable).GetMethods()
                .First(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
                .MakeGenericMethod(memberType);
            return Expression.Call(contains, arrConst, member);
        }
        return Expression.Constant(true); // Nessun valore -> non filtra
    }

    private static object? ConvertTo(object? value, Type targetType)
    {
        if (value == null) return null;
        if (targetType == typeof(string)) return Convert.ToString(value);
        if (targetType == typeof(Guid)) return value is Guid g ? g : Guid.Parse(Convert.ToString(value)!);
        if (targetType == typeof(int)) return Convert.ToInt32(value);
        if (targetType == typeof(decimal)) return Convert.ToDecimal(value, System.Globalization.CultureInfo.InvariantCulture);
        if (targetType == typeof(DateTime)) return Convert.ToDateTime(value, System.Globalization.CultureInfo.InvariantCulture);
        if (targetType == typeof(bool)) return Convert.ToBoolean(value);
        return System.Convert.ChangeType(value, targetType);
    }
}


