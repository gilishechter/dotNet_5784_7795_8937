using System.Reflection;
using System.Text;

namespace BO;

public static class Tools
{
   /// <summary>
   /// add all the object properties to one long string
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="obj"></param>
   /// <returns></returns>
    public static string ToStringProperty<T>(this T obj)
    {
        var properties = typeof(T).GetProperties();//get the type
        StringBuilder resultBuilder = new();//build empty string
        resultBuilder.Append($"{typeof(T).Name} properties:\n");//add the title to the list

        foreach (var prop in properties)
        {
            object propValue = prop.GetValue(obj)!;//get the value of the prop
            if (propValue is IEnumerable<object> collectionValue)//if the prop is a IEnumerable
            {
                // Handle collection property
                resultBuilder.Append($"{prop.Name}: [{string.Join(", ", collectionValue)}]\n");//add all the list to the string
            }
            else
            {
                // Handle non-collection property
                resultBuilder.Append($"{prop.Name}: {propValue}\n");//add the reguler props
            }
        }

        return resultBuilder.ToString();//return the string
    }

}


