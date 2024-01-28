namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Task BoTask)
    {
        Do.Task DoTask = new Do.Task(BoTask.Id, BoTask.IdWorker, BoTask.Name, BoTask.Description, BoTask.MileStone,
                                     BoTask.Time,BoTask.CreateDate,BoTask.WantedStartDate,BoTask.StartDate, BoTask.EndingDate,
                                     BoTask.DeadLine, BoTask.Product, BoTask.Notes, BoTask.Rank);
        try
        {
            int idTask = _dal.Task.Create(DoTask);
            return idTask;
        }
        catch (Do.DalAlreadyExistsException ex)
        {
            throw new Exception($"task with ID={BoTask.Id} already exists", ex);
        }
    }

    public void Delete(int _Id)
    {
        try
        {
            _dal.Task.Delete(_Id);
        }
        catch (Do.DalAlreadyExistsException ex)
        {
            throw new Exception($"Task with ID={_Id} doesn't exists", ex);
        }
    }

    public BO.Task? Read(int _Id)
    {
        Do.Task DoTask = _dal.Task.Read(_Id)!;
        if (DoTask == null)
            throw new Exception($"task with ID={_Id} doesn't exists");
        return new BO.Task()
        {
            Id = _Id,
            WorkerRank
        }
    }
    public IEnumerable<BO.Task> ReadAll(Func<System.Threading.Tasks.Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task task)
    {
        throw new NotImplementedException();
    }
}
