

namespace Dal;

using DalApi;
using Do;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";

    public void ClearList()
    {
        List<Task> list = new List<Task>();
        XMLTools.SaveListToXMLSerializer(list, s_tasks_xml);
    }
    public int Create(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        int nextId = Config.NextTaskId;
        Task newTask = item with { Id= nextId };
        tasks.Add(newTask);
        XMLTools.SaveListToXMLSerializer(tasks,s_tasks_xml);
        return nextId;
    }

    public void Delete(int Id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        foreach(Task tempTask in tasks)
        {
            if (tempTask.Id == Id)
            {
                tasks.Remove(tempTask);
                XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
                return;
            }
        }
        throw new DalDoesNotExistException($"this task with id={Id} is not exist");//throw exception
    }

    public Task? Read(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        return tasks.FirstOrDefault(filter);
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if (filter == null)//if there is no function to filtering
            return tasks.Select(item => item);//return all the list
        else
            return tasks.Where(filter);//return the list after the filtering by the function
    }

    public void Update(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        foreach (Task tempTask in tasks)
        {
            if (tempTask.Id == item.Id)
            {
                tasks.Remove(tempTask);
                tasks.Add(item);
                XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
                return;
            }
        }
        throw new DalDoesNotExistException($"this task with id={item.Id} is not exist");//throw exception
    }
}
