namespace Dal;
using DalApi;
using Do;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// the implemation of the task interface
/// </summary
internal class TaskImplementation : ITask
{
    /// <summary>
    /// Creates new entity object in DAL
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Task item)
    {
        int newId = DataSource.Config.NextTaskId;//the running number
        Task newItem = item with { Id = newId };//create new item with the new ID
        DataSource.Tasks.Add(newItem);//add to the list
        return newId;

    }
    /// <summary>
    /// Deletes an object by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        for (int i = 0; i < DataSource.Tasks.Count; i++)//go through the list
        {
            if (DataSource.Tasks[i].Id == id)//if the ID's even
            {
                DataSource.Tasks.Remove(DataSource.Tasks[i]);//remove the item from the list
                return;
            }
        }
        throw new DalDoesNotExistException($"this task with id={id} is not exist");//throw exception
    }
    /// <summary>
    /// Reads entity object by its ID 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(obj => obj.Id == id);//return the first object from
                                                                    //the list that it's ID Equal to the wanted ID
    }
    /// <summary>
    /// return all the list or according to the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null) 
    {
        if (filter == null)//if there is no function to filtering
            return DataSource.Tasks.Select(item => item);//return all the list
        else
            return DataSource.Tasks.Where(filter);//return the list after the filtering by the function
    }

    /// <summary>
    /// Updates entity object
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Task item)
    {

        foreach (Task task1 in DataSource.Tasks)//go through the list
        {
            if (task1.Id == item.Id)//if the ID's equal
            {
                DataSource.Tasks.Remove(task1);//remove the old item
                DataSource.Tasks.Add(item);//add the new one
                return;
            }
        }
        throw new DalDoesNotExistException($"this task with id={item.Id} is not exist");//throw exception
    }
    /// <summary>
    /// return the object that fits to the function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(filter);

    }
    public void ClearList()
    {
        DataSource.Tasks.Clear();
    }

   
}
