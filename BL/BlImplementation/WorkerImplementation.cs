namespace BlImplementation;
using System.Net.Mail;
using BlApi;
using BO;

internal class WorkerImplementation : IWorker
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Worker boWorker)
    {
        if (boWorker.Id <= 0)
            throw new FormatException("ID can't be negetive number");
        if (boWorker.Name == "")
            throw new FormatException("you must enter a name");
        if (boWorker.HourPrice <= 0)
            throw new FormatException("hour price can't be negetive number");
        if ((boWorker.Email == "") || (!boWorker.Email!.Contains("@gmail.com")))
            throw new FormatException("you must enter an email");
      
        Do.Worker Doworker = new (boWorker.Id, (Do.Rank)boWorker.WorkerRank, boWorker.HourPrice, boWorker.Name, boWorker.Email);
        boWorker.WorkerTask = GetWorkerTask(Doworker);
        try
        {
            int idWorker = _dal.Worker.Create(Doworker);
            return idWorker;
        }
        catch (Do.DalAlreadyExistsException ex)
        {
            throw new BlAlreadyExistsException($"worker with ID={boWorker.Id} already exists", ex);

        }
    }

    public void Delete(int _Id)
    {
        BO.Worker boWorker = Read(_Id)!;

        if (boWorker.WorkerTask.Id is int taskId)
        {
            var doTask = _dal.Task.Read(taskId);
            if (doTask is not null)
            {
                if (GetStatus(doTask) == BO.Status.OnTrackStarted || GetStatus(doTask) == BO.Status.Done)
                    throw new BlCantBeDeleted($"this worker with ID={_Id} is in the middle of task, or has finished it, so he can't be deleted");
                _dal.Task.Update(doTask with { IdWorker = null });
            }
        }
            try
            {
                _dal.Worker.Delete(_Id);
            }
            catch (Do.DalDoesNotExistException ex)
            {
                throw new BlDoesNotExistException($"Worker with ID={_Id} doesnt exists", ex);
            }
        
    }

    public static BO.Status GetStatus(Do.Task task)
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
    public BO.Worker? Read(int id)
    {
        Do.Worker doWorker = _dal.Worker.Read(id)!;

        if (doWorker == null)
            throw new BlDoesNotExistException($"worker with ID={id} doesnt exists");

        return new BO.Worker()
        {
            Id = id,
            WorkerRank = (BO.Rank)doWorker.WorkerRank,
            HourPrice = doWorker.HourPrice,
            Name = doWorker.Name,
            Email = doWorker.Email,
            WorkerTask = GetWorkerTask(doWorker)
        };
    }

    public IEnumerable<BO.Worker> ReadAll(Func<BO.Worker, bool>? filter = null)
    {
        var result = (from Do.Worker doWorker in _dal.Worker.ReadAll()
                      let task = _dal.Task.Read(tmp => tmp.IdWorker == doWorker.Id && GetStatus(tmp) is Status.OnTrackStarted)
                      select new BO.Worker()
                      {
                          Id = doWorker.Id,
                          WorkerRank = (BO.Rank)doWorker.WorkerRank,
                          HourPrice = doWorker.HourPrice,
                          Name = doWorker.Name,
                          Email = doWorker.Email,

                          WorkerTask = new WorkerTask
                          {
                              Id = task != null ? task.Id : null,
                              Name = task != null ? task.Name : null
                          }

                      });
        var orderResult = (from BO.Worker doWorker in result
                           orderby doWorker.Name
                           select doWorker);
        return orderResult;
    }

    public void Update(BO.Worker boWorker)
    {
        BO.Worker oldWorker = Read(boWorker.Id)!;

        if ((boWorker.Email != "") && boWorker.HourPrice <= 0)
            throw new FormatException("hour price can't be negetive number");

        if ((boWorker.Email != "") && (!boWorker.Email!.Contains("@gmail.com")))
            throw new FormatException("you must enter an email");

        if ((boWorker.Email != "") && oldWorker.WorkerRank > boWorker.WorkerRank)
            throw new FormatException("worker rank can only increase");

        if (boWorker.WorkerTask.Id is int taskId)
        {
            var doTask = _dal.Task.Read(taskId);
            if (doTask is not null)
            {
                doTask = doTask with { IdWorker = boWorker.Id };
                _dal.Task.Update(doTask);
            }
        }
        Do.Worker Doworker = new (boWorker.Id, (Do.Rank)boWorker.WorkerRank, boWorker.HourPrice,
            boWorker.Name, boWorker.Email);
        boWorker.WorkerTask = GetWorkerTask(Doworker);

        try
        {
            _dal.Worker.Update(Doworker);
        }
        catch (Do.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"worker with ID={Doworker.Id} doesnt exists", ex);
        }
    }

    public IEnumerable<BO.Worker> RankGroup(Rank rank)
    {
        var result = (from Do.Worker doWorker in _dal.Worker.ReadAll()
                      group doWorker by rank into rankList
                      select rankList);

        var result1 = (from Do.Worker doWorker in result
                       let task = _dal.Task.Read(tmp => tmp.IdWorker == doWorker.Id && GetStatus(tmp) is Status.OnTrackStarted)
                       select new BO.Worker
                       {
                           Id = doWorker.Id,
                           WorkerRank = (BO.Rank)doWorker.WorkerRank,
                           HourPrice = doWorker.HourPrice,
                           Name = doWorker.Name,
                           Email = doWorker.Email,
                        
                           WorkerTask = new WorkerTask
                           {
                               Id = task != null ? task.Id : null,
                               Name = task != null ? task.Name : null
                           }
                       });
        return result1;
    }


    public WorkerTask? GetWorkerTask(Do.Worker doWorker)
    {
        Do.Task? task = _dal.Task.Read(tmp => tmp.IdWorker == doWorker.Id && GetStatus(tmp) is Status.OnTrackStarted);
        WorkerTask current = new()
        {
            Id = task != null ? task.Id : null,
            Name = task != null ? task.Name : null
        };
        return current;
    }

    public void ClearWorker()
    {
        _dal.Worker.ClearList();
    }
}
