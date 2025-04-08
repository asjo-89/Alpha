using System.Reflection;

namespace Domain.Extensions;

public static class MapExtensions
{
    public static TDestination MapTo<TDestination>(this object source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        TDestination destination = (TDestination)Activator.CreateInstance(typeof(TDestination))!;
        var sourceProperties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var destinationProperties = destination.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach(var property in destinationProperties)
        {
            var sourceProperty = sourceProperties.FirstOrDefault(x => x.Name == property.Name && x.PropertyType == property.PropertyType);
            if (sourceProperty != null)
            {
                var value = sourceProperty.GetValue(source);
                property.SetValue(destination, value);
            }
        }
        return destination;
    }
}
