using System.Reflection;
using System.Text;

namespace BO;

public static class Tools
{
    //public static string ToStringProperty<T>(this T obj)
    //{
    //    var properties = typeof(T).GetProperties();
    //    string result = $"{typeof(T).Name} properties:\n";

    //    foreach (var prop in properties)
    //    {
    //        result += $"{prop.Name}: {prop.GetValue(obj)}\n";
    //    }

    //    return result;
    //}     

    public static string ToStringProperty<T>(this T obj)
    {
        var properties = typeof(T).GetProperties();
        string result = $"{typeof(T).Name} properties:\n";

        foreach (var prop in properties)
        {
            if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                // Handle collection property
                var collectionValue = prop.GetValue(obj) as IEnumerable<object>;
                if (collectionValue != null)
                {
                    result += $"{prop.Name}: [";
                    foreach (var item in collectionValue)
                    {
                        result += $"{item}, ";
                    }
                    // Remove the trailing comma and space
                    if (result.EndsWith(", "))
                    {
                        result = result.Remove(result.Length - 2);
                    }
                    result += "]\n";
                }
            }
            else
            {
                // Handle non-collection property
                result += $"{prop.Name}: {prop.GetValue(obj)}\n";
            }
        }

        return result;
    }

}


