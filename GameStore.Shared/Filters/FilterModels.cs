namespace GameStore.Shared.Filters;

/// <summary>
/// Tipi di campo supportati per conversioni e confronti.
/// </summary>
public enum FieldType
{
    String,
    Decimal,
    Integer,
    Guid,
    DateTime,
    Boolean
}

/// <summary>
/// Operatori di confronto supportati.
/// </summary>
public enum ComparisonOperator
{
    Equals,
    NotEquals,
    Contains,
    StartsWith,
    EndsWith,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    In,
    Between,
    IsNull,
    IsNotNull
}

/// <summary>
/// Operatori logici di raggruppamento.
/// </summary>
public enum LogicalOperator
{
    And,
    Or
}

/// <summary>
/// Un singolo criterio di filtro su un campo.
/// </summary>
public class FilterCondition
{
    public string Field { get; set; } = string.Empty;
    public FieldType FieldType { get; set; } = FieldType.String;
    public ComparisonOperator Operator { get; set; } = ComparisonOperator.Equals;
    public object? Value { get; set; }
    public object? Value2 { get; set; } // per intervalli (Between)
}

/// <summary>
/// Un gruppo di filtri con operatore logico e possibilità di nidificazione.
/// </summary>
public class FilterGroup
{
    public LogicalOperator Logic { get; set; } = LogicalOperator.And;
    public List<FilterCondition> Conditions { get; set; } = new();
    public List<FilterGroup> Groups { get; set; } = new();
}


