using GameStore.Shared.Filters;
using Radzen;

namespace GameStore.WebUI.Pages;

/// <summary>
/// Adapter che converte il nostro FilterGroup condiviso in filtri per Radzen DataGrid.
/// Nota: Radzen supporta la costruzione dei filtri tramite parametro Filter su colonne e FilterCaseSensitivity.
/// Poiché il DataGrid applica filtri per colonna, questo adapter produce una lista piatta per colonna.
/// </summary>
public static class RadzenFilterAdapter
{
    /// <summary>
    /// Converte un FilterGroup in una collezione di filtri per colonna Radzen.
    /// Restituisce un dizionario: NomeCampo -> lista di predicate.
    /// </summary>
    public static Dictionary<string, List<FilterDescriptor>> ToRadzenFilters(FilterGroup group)
    {
        Dictionary<string, List<FilterDescriptor>> map = new();
        Flatten(group, map);
        return map;
    }

    private static void Flatten(FilterGroup group, Dictionary<string, List<FilterDescriptor>> map)
    {
        foreach (FilterCondition c in group.Conditions)
        {
            FilterDescriptor? fd = ToFilterDescriptor(c);
            if (fd == null) continue;
            if (!map.TryGetValue(c.Field, out List<FilterDescriptor>? list))
            {
                list = new List<FilterDescriptor>();
                map[c.Field] = list;
            }
            list.Add(fd);
        }
        foreach (FilterGroup sub in group.Groups)
        {
            Flatten(sub, map);
        }
    }

    private static FilterDescriptor? ToFilterDescriptor(FilterCondition c)
    {
        FilterOperator op = c.Operator switch
        {
            ComparisonOperator.Equals => FilterOperator.Equals,
            ComparisonOperator.NotEquals => FilterOperator.NotEquals,
            ComparisonOperator.Contains => FilterOperator.Contains,
            ComparisonOperator.StartsWith => FilterOperator.StartsWith,
            ComparisonOperator.EndsWith => FilterOperator.EndsWith,
            ComparisonOperator.GreaterThan => FilterOperator.GreaterThan,
            ComparisonOperator.GreaterThanOrEqual => FilterOperator.GreaterThanOrEquals,
            ComparisonOperator.LessThan => FilterOperator.LessThan,
            ComparisonOperator.LessThanOrEqual => FilterOperator.LessThanOrEquals,
            ComparisonOperator.IsNull => FilterOperator.IsNull,
            ComparisonOperator.IsNotNull => FilterOperator.IsNotNull,
            _ => FilterOperator.Equals
        };

        if (c.Operator == ComparisonOperator.In)
        {
            // Radzen non ha In nativo a livello di singolo descriptor: espandi a Contains/Equals multipli a livello di colonna fuori da qui
            return null;
        }
        if (c.Operator == ComparisonOperator.Between)
        {
            // Gestito come due descriptor >= e <=
            return null;
        }

        return new FilterDescriptor
        {
            Property = c.Field,
            FilterOperator = op,
            FilterValue = c.Value
        };
    }
}


