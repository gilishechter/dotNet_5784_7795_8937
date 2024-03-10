using BlApi;
using DalApi;
using System.Reflection;
using System.Text;

namespace BO;

public static class Tools
{

    private static readonly IBl _bl = BlApi.Factory.Get();
    private static readonly IDal dal = DalApi.Factory.Get;
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




    public static int GetOfset(Do.Task task)
    {
        IEnumerable<Do.Dependency> dependencies = dal.Dependency.ReadAll(x => x.DependenceTask == task.Id);
        if (dependencies.Count() == 0)
            return (task.StartDate != null ? (int)(task.StartDate - _bl.GetStartProject())?.TotalDays! :
                                            (int)(task.WantedStartDate - _bl.GetStartProject())?.TotalDays!) * 100;

        int x = ((int)((from dep in dependencies
                        let depTask = dal.Task.Read(dep.PrevTask)
                        select depTask.WantedStartDate + depTask.Time).Max() - _bl.GetStartProject())?.TotalDays!) * 100;
        return x;
    }
}