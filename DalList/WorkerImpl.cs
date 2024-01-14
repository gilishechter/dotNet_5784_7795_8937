namespace Dal;
using DalApi;
using Do;
using System.Collections.Generic;

/* Work implementation clsdd*/
internal class WorkerImplementation : IWorker
{
    public int Create(Worker item)
    {
        foreach (Worker Worker1 in DataSource.Workers)
        {
            if (Worker1.Id == item.Id)
            {
                throw new DalAlreadyExistsException($"this worker with id={item.Id} is already exist");
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
        throw new DalDoesNotExistException($"this worker with id={id} is not exist");
    }

    public Worker? Read(int id)
    {
        return DataSource.Workers.FirstOrDefault(obj => obj.Id == id);
    }

    public IEnumerable<Worker?> ReadAll(Func<Worker, bool>? filter = null) //stage 2
    {
        if (filter == null)
            return DataSource.Workers.Select(item => item);
        else
            return DataSource.Workers.Where(filter);
    }


    public void Update(Worker item)
    {
        foreach (Worker worker1 in DataSource.Workers)
        {
            if (worker1.Id == item.Id)
            {
                DataSource.Workers.Remove(worker1);
                DataSource.Workers.Add(item);
                return;
            }
        }
        throw new DalDoesNotExistException($"this worker with id={item.Id} is not exist");
    }

    public Worker? Read(Func<Worker, bool> filter)
    {
        foreach (Worker worker1 in DataSource.Workers)
        {
            if(filter(worker1))
                return worker1;
        }
        return null;
    }
}
