using System.Reflection;

namespace TAB.Domain.Core.Utils;

public class PropertyConverter
{
    public static object ConvertPropertyValue(PropertyInfo property, string valueString)
    {
        if (property.PropertyType.IsEnum)
        {
            if (Enum.TryParse(property.PropertyType, valueString, true, out var enumValue))
                return enumValue;
            throw new ArgumentException("Invalid enum value.");
        }

        return Convert.ChangeType(valueString, property.PropertyType);
    }
}
