namespace Dal;
using DalApi;
using Do;
using System.Collections.Generic;

/* Work implementation clsdd*/
public class WorkerImplementation : IWorker
{
    public int Create(Worker item)
    {
        foreach (Worker Worker1 in DataSource.Workers)
        {
            if (Worker1.Id == item.Id)
            {
                throw new Exception($"this worker with id={item.Id} is already exist");
            }
        }
       
        DataSource.Workers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        for (int i = 0; i < DataSource.Workers.Count; i++)
        {
            if (DataSource.Workers[i].Id == id)
            {
                DataSource.Workers.Remove(DataSource.Workers[i]);
                return;
            }
        }
        throw new Exception($"this worker with id={id} is not exist");
    }

    public Worker? Read(int id)
    {
        Worker? findWorker = DataSource.Workers.Find(obj => obj.Id == id);
        return findWorker;
    }

    public List<Worker> ReadAll()
    {
        return new List<Worker>(DataSource.Workers);
    }

    public void Update(Worker item)
    {
        for (int i = 0; i < DataSource.Workers.Count; i++)
        {
            if (DataSource.Workers[i] == item)
            {
                DataSource.Workers.Remove(DataSource.Workers[i]);
                DataSource.Workers.Add(item);
                return;
            }
        }
        throw new Exception($"this worker with id={item.Id} is not exist");
    }
}
