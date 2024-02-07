

namespace Dal;

using DalApi;
using Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";
    /// <summary>
    /// clear the list in the XML file and update the run number to start from the begining 
    /// </summary>
    public void ClearList()
    {
        List<Task> tasks = new();
        Config.NextTaskId = 1;// update the run number to start from 1
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);// save the list in XML       
    }
    /// <summary>
    /// Creates new entity object in XML file
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        int nextId = Config.NextTaskId;
        Task newTask = item with { Id= nextId };
        tasks.Add(newTask);//add the new task
        XMLTools.SaveListToXMLSerializer(tasks,s_tasks_xml);
        return nextId;
    }
    /// <summary>
    /// Deletes an object by its Id
    /// </summary>
    /// <param name="Id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int Id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        foreach(Task tempTask in tasks)//Goes through all the tasks and looking for the same id to delete
        {
            if (tempTask.Id == Id)
            {
                tasks.Remove(tempTask);//remove if he find the task to delete
                XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
                return;
            }
        }
        throw new DalDoesNotExistException($"this task with id={Id} is not exist");//throw exception
    }

  

    /// <summary>
    /// Reads entity object by its ID 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task? Read(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        return tasks.FirstOrDefault(t => t.Id == id);//Returns the first task that meets the condition
    }
    /// <summary>
    /// return all the list or according to the filter 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        return tasks.FirstOrDefault(filter);
    }
    /// <summary>
    /// return all the list or according to the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if (filter == null)//if there is no function to filtering
            return tasks.Select(item => item);//return all the list
        else
            return tasks.Where(filter);//return the list after the filtering by the function
    }


    /// <summary>
    /// Updates entity object in the XML file
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        foreach (Task tempTask in tasks)//Goes through all the tasks and looking for the same id to delete
        {
            if (tempTask.Id == item.Id)
            {
                tasks.Remove(tempTask);//remove the old task
                tasks.Add(item);//add the new task with the update details
                XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
                return;
            }
        }
        throw new DalDoesNotExistException($"this task with id={item.Id} is not exist");//throw exception
    }
}
