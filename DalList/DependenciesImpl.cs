namespace Dal;
using DalApi;
using Do;
using System.Collections.Generic;

internal class DependenceImplementation : IDependence
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
        throw new DalDoesNotExistException($"this dependence with id={id} is not exist");
    }

    public Dependencies? Read(int id)
    {
      
        return DataSource.Dependencies.FirstOrDefault(d => d.Id == id);
    }

    public IEnumerable<Dependencies?> ReadAll(Func<Dependencies, bool>? filter = null) //stage 2
    {
        if (filter == null)
            return DataSource.Dependencies.Select(item => item);
        else
            return DataSource.Dependencies.Where(filter);
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
        throw new DalDoesNotExistException($"this dependence with id={item.Id} is not exist");
       
    }

    public Dependencies? Read(Func<Dependencies, bool> filter)
    {
        foreach (Dependencies dependence1 in DataSource.Dependencies)
        {
            if(filter(dependence1))
                return dependence1;
        }
        return null;
    }
}
