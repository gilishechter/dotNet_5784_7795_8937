namespace Dal;
using DalApi;
using Do;
using System.Collections.Generic;
/// <summary>
/// the implemation of the dependence interface
/// </summary>
internal class DependenceImplementation : IDependence
{
    /// <summary>
    /// Creates new entity object in DAL
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependencies item)
    {
        int newId = DataSource.Config.NextDependenceId;//the running number
        Dependencies newItem = item with { Id = newId };//create new item with the new ID
        DataSource.Dependencies.Add(newItem);//add to the list
        return newId;
    }
    /// <summary>
    /// Deletes an object by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        for (int i = 0; i < DataSource.Dependencies.Count; i++)//go through the list
        {
            if (DataSource.Dependencies[i].Id == id)//if the ID's even
            {
                DataSource.Dependencies.Remove(DataSource.Dependencies[i]);//remove the item from the list
                return;
            }
        }
        throw new DalDoesNotExistException($"this dependence with id={id} is not exist");//throw exception
    }

    /// <summary>
    /// Reads entity object by its ID 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependencies? Read(int id)
    {
      
        return DataSource.Dependencies.FirstOrDefault(d => d.Id == id);//return the first object from
                                                                       //the list that it's ID Equal to the wanted ID
    }
    /// <summary>
    /// return all the list or according to the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Dependencies?> ReadAll(Func<Dependencies, bool>? filter = null) 
    {
        if (filter == null)//if there is no function to filtering
            return DataSource.Dependencies.Select(item => item);//return all the list
        else
            return DataSource.Dependencies.Where(filter);//return the list after the filtering by the function
    }

    /// <summary>
    /// Updates entity object
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Dependencies item)
    {
        foreach (Dependencies dependence1 in DataSource.Dependencies)//go through the list
        {
            if (dependence1.Id == item.Id)//if the ID's equal
            {
                DataSource.Dependencies.Remove(dependence1);//remove the old item
                DataSource.Dependencies.Add(item);//add the new one
                return;
            }
        }
        throw new DalDoesNotExistException($"this dependence with id={item.Id} is not exist");//throw exception
       
    }
    /// <summary>
    /// return the object that fits to the function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Dependencies? Read(Func<Dependencies, bool> filter)
    {
        foreach (Dependencies dependence1 in DataSource.Dependencies)//go through the list
        {
            if(filter(dependence1))//if the function return true
                return dependence1;//return the item
        }
        return null;
    }
}
