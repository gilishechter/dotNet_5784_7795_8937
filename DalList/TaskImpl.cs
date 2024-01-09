namespace Dal;
using DalApi;
using Do;
using System.Collections.Generic;

public class TaskImplementation : ITask
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
        throw new Exception($"this task with id={id} is not exist");
    }

    public Task? Read(int id)
    {
        Task? findTask = DataSource.Tasks.Find(obj => obj.Id == id);
        return findTask;
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
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
        throw new Exception($"this task with id={item.Id} is not exist");
    }
}
