namespace Dal;
using DalApi;
using Do;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int newId = DataSource.Config.NextTaskId;
        Task newItem = item with { Id = newId };
        DataSource.Tasks.Add(newItem);
        return newId;

    }

    public void Delete(int id)
    {
        for (int i = 0; i < DataSource.Tasks.Count; i++)
        {
            if (DataSource.Tasks[i].Id == id)
            {
                DataSource.Tasks.Remove(DataSource.Tasks[i]);
                return;
            }
        }
        throw new DalDoesNotExistException($"this task with id={id} is not exist");
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(obj => obj.Id == id);
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null) //stage 2
    {
        if (filter == null)
            return DataSource.Tasks.Select(item => item);
        else
            return DataSource.Tasks.Where(filter);
    }


    public void Update(Task item)
    {

        foreach (Task task1 in DataSource.Tasks)
        {
            if (task1.Id == item.Id)
            {
                DataSource.Tasks.Remove(task1);
                DataSource.Tasks.Add(item);
                return;
            }
        }
        throw new DalDoesNotExistException($"this task with id={item.Id} is not exist");
    }

    public Task? Read(Func<Task, bool> filter)
    {
        foreach (Task task1 in DataSource.Tasks)
        {
            if(filter(task1))
                return task1;
        }
        return null;
    }
}
