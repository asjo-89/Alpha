using System.Collections;
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

                // Lagt till/modifierat delarna markerade med * med hjälp av GitHubCopilot för att möjliggöra mappning av komplexa fält såsom listor eller där fältet är en annan typ av objekt/string.
                // Hämtar metoden som generisk och invokar den för att kunna mappa dessa fält med propertyType.

                // * start
                // Möjliggör att komplexa fälttyper kan mappas genom generisk mapmetod för just det objekt/klass fältet har.
                if (value != null && !property.PropertyType.IsPrimitive && property.PropertyType != typeof(string))
                {
                    var mapToMethod = typeof(MapExtensions).GetMethod("MapTo", BindingFlags.Public | BindingFlags.Static)
                        ?.MakeGenericMethod(property.PropertyType);

                    var mappedValue = mapToMethod?.Invoke(null, [value]);
                    property.SetValue(destination, mappedValue);
                }
                // * slut

                else
                {
                    property.SetValue(destination, value);
                }

                // * start
                // Möjliggör att listor eller samlingar kan mappas genom generisk metod.
                if (value is IEnumerable<object> enumerable && property.PropertyType.IsGenericType)
                {
                    // Hämtar typen av listan/samlingen
                    var listType = property.PropertyType.GetGenericArguments()[0];
                    // Skapar en lista baserat på den hämtade listtypen
                    var mappedList = Activator.CreateInstance(typeof(List<>).MakeGenericType(listType)) as IList;

                    foreach (var item in enumerable)
                    {
                        var mapToMethod = typeof(MapExtensions).GetMethod("MapTo")!.MakeGenericMethod(listType);
                        var mappedItem = mapToMethod.Invoke(null, [item]);
                        mappedList?.Add(mappedItem);
                    }

                    property.SetValue(destination, mappedList);
                }
                // *
            }
        }
        return destination;
    }
}
