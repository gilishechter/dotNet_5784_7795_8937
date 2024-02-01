namespace Dal;

using DalApi;
using Do;
using System;
using System.Collections.Generic;
using System.Linq;

internal class WorkerImplementation : IWorker
{
    readonly string s_workers_xml = "workers";

    public void ClearList()
    {
        List<Worker> workers = new();
        workers.Clear();
        XMLTools.SaveListToXMLSerializer(workers, s_workers_xml);
    }
    public int Create(Worker item)
    {
        List<Worker> workers = XMLTools.LoadListFromXMLSerializer<Worker>(s_workers_xml);
        foreach (Worker tempWorker in workers)//go through the list
        {
            if (tempWorker.Id == item.Id)//if the item is already exist
            {
                throw new DalAlreadyExistsException($"this worker with id={item.Id} is already exist");//throe exception
            }
        }

        workers.Add(item);//add the object to the list
        XMLTools.SaveListToXMLSerializer(workers, s_workers_xml);
        return item.Id;
    }

    public void Delete(int Id)
    {
        List<Worker> workers = XMLTools.LoadListFromXMLSerializer<Worker>(s_workers_xml);
        foreach (Worker tempWorker in workers)
        {
            if (tempWorker.Id == Id)
            {
                workers.Remove(tempWorker);
                XMLTools.SaveListToXMLSerializer(workers, s_workers_xml);
                return;
            }
        }
        throw new DalDoesNotExistException($"this worker with id={Id} is not exist");//throw exception
    }

    public Worker? Read(int id)
    {
        List<Worker> workers = XMLTools.LoadListFromXMLSerializer<Worker>(s_workers_xml);
        return workers.FirstOrDefault(t => t.Id == id);
    }

    public Worker? Read(Func<Worker, bool> filter)
    {
        List<Worker> workers = XMLTools.LoadListFromXMLSerializer<Worker>(s_workers_xml);
        return workers.FirstOrDefault(filter);
    }

    public IEnumerable<Worker?> ReadAll(Func<Worker, bool>? filter = null)
    {
        List<Worker> workers = XMLTools.LoadListFromXMLSerializer<Worker>(s_workers_xml);
        if (filter == null)//if there is no function to filtering
            return workers.Select(item => item);//return all the list
        else
            return workers.Where(filter);//return the list after the filtering by the function
    }

    public void Update(Worker item)
    {
        List<Worker> workers = XMLTools.LoadListFromXMLSerializer<Worker>(s_workers_xml);
        foreach (Worker tempWorker in workers)
        {
            if (tempWorker.Id == item.Id)
            {
                workers.Remove(tempWorker);
                workers.Add(item);
                XMLTools.SaveListToXMLSerializer(workers, s_workers_xml);
                return;
            }
        }
        throw new DalDoesNotExistException($"this worker with id={item.Id} is not exist");//throw exception
    }
}

