namespace Dal;
using DalApi;
using Do;
using System.Collections.Generic;

/// <summary>
/// the implemation of the worker interface
/// </summary>
internal class WorkerImplementation : IWorker
{
    /// <summary>
    /// Creates new entity object in DAL
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Worker item)
    {
        foreach (Worker Worker1 in DataSource.Workers)//go through the list
        {
            if (Worker1.Id == item.Id)//if the item is already exist
            {
                throw new DalAlreadyExistsException($"this worker with id={item.Id} is already exist");//throe exception
            }
        }
       
        DataSource.Workers.Add(item);//add the object to the list
        return item.Id;
    }
    /// <summary>
    /// Deletes an object by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        for (int i = 0; i < DataSource.Workers.Count; i++)//go through the list
        {
            if (DataSource.Workers[i].Id == id)//if the ID's even
            {
                DataSource.Workers.Remove(DataSource.Workers[i]);//remove the item from the list
                return;
            }
        }
        throw new DalDoesNotExistException($"this worker with id={id} is not exist");//throw exception
    }
    /// <summary>
    /// Reads entity object by its ID 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Worker? Read(int id)
    {
        return DataSource.Workers.FirstOrDefault(obj => obj.Id == id);//return the first object from
                                                                      //the list that it's ID Equal to the wanted ID
    }
    /// <summary>
    /// return all the list or according to the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Worker?> ReadAll(Func<Worker, bool>? filter = null) 
    {
        if (filter == null)//if there is no function to filtering
            return DataSource.Workers.Select(item => item);//return all the list
        else
            return DataSource.Workers.Where(filter);//return the list after the filtering by the function
    }

    /// <summary>
    /// Updates entity object
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Worker item)
    {
        foreach (Worker worker1 in DataSource.Workers)//go through the list
        {
            if (worker1.Id == item.Id)//if the ID's equal
            {
                DataSource.Workers.Remove(worker1);//remove the old item
                DataSource.Workers.Add(item);//add the new one
                return;
            }
        }
        throw new DalDoesNotExistException($"this worker with id={item.Id} is not exist");//throw exception
    }
    /// <summary>
    /// return the object that fits to the function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Worker? Read(Func<Worker, bool> filter)
    {
        foreach (Worker worker1 in DataSource.Workers)//go through the list
        {
            if(filter(worker1))//if the function return true
                return worker1;//return the item
        }
        return null;
    }
}
