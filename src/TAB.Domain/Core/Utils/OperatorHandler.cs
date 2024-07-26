namespace TAB.Domain.Core.Utils;

public class OperatorHandler
{
    public static (string? propertyName, string? operatorString, string? valueString) Parse(
        string filter
    )
    {
        var operators = ValidOperators.AllOperators.OrderByDescending(op => op.Length).ToArray();

        var operatorIndex = -1;
        string? operatorString = null;

        foreach (var op in operators)
        {
            var index = filter.IndexOf(op, StringComparison.OrdinalIgnoreCase);
            if (index >= 0 && (operatorIndex == -1 || index < operatorIndex))
            {
                operatorIndex = index;
                operatorString = op;
            }
        }

        if (operatorIndex < 0 || operatorString == null)
            return (null, null, null);

        var propertyName = filter[..operatorIndex].Trim();
        var valueString = filter[(operatorIndex + operatorString.Length)..].Trim();

        return (propertyName, operatorString, valueString);
    }
}
