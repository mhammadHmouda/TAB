using System.Reflection;

namespace TAB.Domain.Core.Utils;

public class PropertyConverter
{
    public static object ConvertPropertyValue(PropertyInfo property, string valueString)
    {
        var propertyType = property.PropertyType;

        if (Nullable.GetUnderlyingType(propertyType) != null)
        {
            propertyType = Nullable.GetUnderlyingType(propertyType);
        }

        if (propertyType == null)
            throw new ArgumentException("Invalid property type.");

        if (propertyType == typeof(string))
            return valueString;

        if (propertyType.IsEnum)
        {
            if (Enum.TryParse(propertyType, valueString, true, out var enumValue))
                return enumValue;
            throw new ArgumentException("Invalid enum value.");
        }

        if (propertyType.IsClass)
            throw new ArgumentException("Unsupported type.");

        return Convert.ChangeType(valueString, propertyType);
    }
}
