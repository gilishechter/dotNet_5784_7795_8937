namespace Dal;
using DalApi;
using Do;
using System.Collections.Generic;

public class DependenceImplementation : IDependence
{
    public int Create(Dependencies item)
    {
        int newId = DataSource.Config.NextDependenceId;
        Dependencies newItem = item with { Id = newId };
        DataSource.Dependencies.Add(newItem);
        return newId;
    }

    public void Delete(int id)
    {
        for (int i = 0; i < DataSource.Dependencies.Count; i++)
        {
            if (DataSource.Dependencies[i].Id == id)
            {
                DataSource.Dependencies.Remove(DataSource.Dependencies[i]);
                return;
            }
        }
        throw new Exception($"this dependence with id={id} is not exist");
    }

    public Dependencies? Read(int id)
    {
        Dependencies? findDependence = DataSource.Dependencies.Find(obj => obj.Id == id);
        return findDependence;
    }

    public List<Dependencies> ReadAll()
    {
        return new List<Dependencies>(DataSource.Dependencies);
    }

    public void Update(Dependencies item)
    {
        foreach (Dependencies dependence1 in DataSource.Dependencies)
        {
            if (dependence1.Id == item.Id)
            {
                DataSource.Dependencies.Remove(dependence1);
                DataSource.Dependencies.Add(item);
                return;
            }
        }
        throw new Exception($"this dependence with id={item.Id} is not exist");
       
    }
}
