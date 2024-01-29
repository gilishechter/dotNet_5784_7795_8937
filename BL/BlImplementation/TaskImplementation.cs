namespace BlImplementation;
using BlApi;
//using BO;
//using DalApi;
//using Do;
//using System;
//using System.Collections.Generic;
//using ITask = BlApi.ITask;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    private IEnumerable<BO.TaskList> getDependenceList(Do.Task doTask)
    {
        var result = (from Do.Dependency dependency in _dal.Dependency.ReadAll()
                      where doTask.Id == dependency.DependenceTask
                      select new BO.TaskList
                      {
                          Id = dependency.PrevTask,
                          Description = _dal.Task.Read(dependency.PrevTask)!.Description,
                          Name = _dal.Task.Read(dependency.PrevTask)!.Name,
                          Status = getStatus(doTask)

                      });
        return result;
    }
    private BO.Status getStatus(Do.Task task)
    {
        var dateTimeNow = DateTime.Now;

        return task switch
        {
            Do.Task t when t.IdWorker is null => BO.Status.Unscheduled,
            Do.Task t when t.StartDate > dateTimeNow => BO.Status.Scheduled,
            Do.Task t when t.EndingDate > dateTimeNow => BO.Status.OnTrackStarted,
            _ => BO.Status.Done,
        };
    }

    public int Create(BO.Task boTask)
    {
        if (boTask.Id < 0)
            throw new FormatException("ID can't be negetive number");

        if(boTask.Name == "")
            throw new FormatException("you must enter a name");
        var result = (from BO.TaskList temp in  boTask.DependenceTasks
                      select new Do.Dependency
                      {
                          Id = 0,
                          DependenceTask = boTask.Id,
                          PrevTask = temp.Id
                      };
                       select _dal.Dependency.Create());
        Do.Task DoTask = new Do.Task(boTask.Id, boTask.IdWorker, boTask.Name, boTask.Description, boTask.MileStone,
                                     boTask.Time,boTask.CreateDate,boTask.WantedStartDate,boTask.StartDate, boTask.EndingDate,
                                     boTask.DeadLine, boTask.Product, boTask.Notes, boTask.Rank);
        try
        {
            int idTask = _dal.Task.Create(DoTask);
            return idTask;
        }
        catch (Do.DalAlreadyExistsException ex)
        {
            throw new Exception($"task with ID={boTask.Id} already exists", ex);
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

    public BO.Task? Read(int id)
    {

        Do.Task doTask = _dal.Task.Read(id)!;

        if (doTask == null)
            throw new BlDoesNotExistException($"Task with ID={id} doesnt exists");

        return new BO.Task()
        {
            Id = id,
            IdWorker=doTask.IdWorker,
            Name=doTask.Name,
            Description=doTask.Description,
            MileStone=doTask.MileStone,
            Time=doTask.Time,
            CreateDate=doTask.CreateDate,
            WantedStartDate=doTask.WantedStartDate,
            StartDate=doTask?.StartDate,
            EndingDate=doTask?.EndingDate,
            DeadLine=doTask!.DeadLine,
            Product=doTask.Product,
            Notes=doTask.Notes,
            Rank =doTask.Rank,
            Status =getStatus(doTask),
            DependenceTasks=getDependenceList(doTask)

        };

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
