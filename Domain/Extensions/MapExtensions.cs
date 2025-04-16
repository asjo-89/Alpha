using System.Collections;
using System.Reflection;

namespace Domain.Extensions;

public static class MapExtensions
{
    public static TDestination MapTo<TDestination>(this object source, TDestination? existingEntity = null) where TDestination : class, new()
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        TDestination destination = existingEntity ?? new TDestination();

        var sourceProperties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var destinationProperties = destination.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in destinationProperties)
        {
            var sourceProperty = sourceProperties.FirstOrDefault(x => x.Name == property.Name);

            if (sourceProperty != null && property.CanWrite)
            {
                var value = sourceProperty.GetValue(source);

                // Hantera listor och samlingar
                if (value is IEnumerable<object> enumerable && property.PropertyType.IsGenericType)
                {
                    var listType = property.PropertyType.GetGenericArguments()[0];
                    var mappedList = Activator.CreateInstance(typeof(List<>).MakeGenericType(listType)) as IList;

                    foreach (var item in enumerable)
                    {
                        var mapToMethod = typeof(MapExtensions).GetMethod("MapTo")!.MakeGenericMethod(listType);
                        var mappedItem = mapToMethod.Invoke(null, new[] { item });
                        mappedList?.Add(mappedItem);
                    }

                    property.SetValue(destination, mappedList);
                }
                // Hantera navigationsegenskaper (komplexa typer)
                else if (value != null && !property.PropertyType.IsPrimitive && property.PropertyType != typeof(string))
                {
                    var mapToMethod = typeof(MapExtensions).GetMethod("MapTo")!.MakeGenericMethod(property.PropertyType);
                    var mappedValue = mapToMethod.Invoke(null, new[] { value });
                    property.SetValue(destination, mappedValue);
                }
                // Hantera främmande nycklar (FK)
                else if (property.Name.EndsWith("Id") && value != null)
                {
                    property.SetValue(destination, value);
                }
                // Hantera enkla typer
                else
                {
                    property.SetValue(destination, value);
                }
            }
        }

        return destination;
    }
}
