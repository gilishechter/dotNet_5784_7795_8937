namespace BlImplementation;
using BlApi;
using System.Security.Cryptography;

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
                          TaskDetails = new BO.WorkerTask
                          {
                              Id = dependency.PrevTask,
                              Name = _dal.Task.Read(dependency.PrevTask)!.Name
                          },                        
                          Description = _dal.Task.Read(dependency.PrevTask)!.Description,
                          Status = getStatus(doTask)

                      }) ;
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

        var result = (from BO.TaskList temp in boTask.DependenceTasks
                      select new Do.Dependency
                      {
                          Id = 0,
                          DependenceTask = boTask.Id,
                          PrevTask = temp.TaskDetails.Id,
                      });

        var result1 = (from Do.Dependency dep in result
                       select _dal.Dependency.Create(dep)); 

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
            throw new BlAlreadyExistsException($"task with ID={boTask.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        var result = (from Do.Dependency dep in _dal.Dependency.ReadAll()
                      where dep.PrevTask == id && _dal.Task.Read(dep.Id) != null&& getStatus(_dal.Task.Read(dep.Id)!) == BO.Status.Done
                      select dep) ;
        if (result.Count() > 0)
            throw new BlCantBeDeleted("this task can't be deleted because it has dependence tasks");


        try
        {
            _dal.Task.Delete(id);
        }
        catch (Do.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Task with ID={id} doesn't exists", ex);
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
        var result = (from Do.Task doTask in _dal.Task.ReadAll()
                      select new BO.Task()
                      {
                          Id = doTask.Id,
                          IdWorker = doTask.IdWorker,
                          Name = doTask.Name,
                          Description = doTask.Description,
                          MileStone = doTask.MileStone,
                          Time = doTask.Time,
                          CreateDate = doTask.CreateDate,
                          WantedStartDate = doTask.WantedStartDate,
                          StartDate = doTask?.StartDate,
                          EndingDate = doTask?.EndingDate,
                          DeadLine = doTask!.DeadLine,
                          Product = doTask.Product,
                          Notes = doTask.Notes,
                          Rank = doTask.Rank,
                          Status = getStatus(doTask),
                          DependenceTasks = getDependenceList(doTask)
                      } );
        var orderResult = (from BO.Task doTask in result
                           orderby doTask.Name
                           select doTask);
        return orderResult;
    }

    public void Update(BO.Task boTask)
    {
        if (boTask.Id < 0)
            throw new FormatException("ID can't be negetive number");

        if (boTask.Name == "")
            throw new FormatException("you must enter a name");

        Do.Task doTask = new Do.Task(boTask.Id, boTask.IdWorker, boTask.Name, boTask.Description, boTask.MileStone,
                                    boTask.Time, boTask.CreateDate, boTask.WantedStartDate, boTask.StartDate, boTask.EndingDate,
                                    boTask.DeadLine, boTask.Product, boTask.Notes, boTask.Rank);

        try
        {
            CheckUpdateDate(boTask.Id, boTask.StartDate);
            _dal.Task.Update(doTask);
        }
        catch (Do.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"task with ID={doTask.Id} doesnt exists", ex);
        }
    }

    private void CheckUpdateDate(int idTask, DateTime? date)
    {
        if (_dal.Task.Read(idTask) == null)
            throw new BlDoesNotExistException($"task with ID={idTask} doesnt exists");
        Do.Task? doTask = _dal.Task.Read(idTask);
        var result = (from BO.Task boTask in getDependenceList(doTask)
                      where boTask.WantedStartDate == null || date < boTask.DeadLine
                      select boTask);
        if (result.Count() > 0)
            throw new BlCantBeUpdated("this date can't be updated");

    }
}
