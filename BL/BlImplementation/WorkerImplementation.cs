namespace BlImplementation;
using BlApi;
using BO;

internal class WorkerImplementation : IWorker
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Worker Boworker)
    {
        if (Boworker.Id <= 0)
            throw new FormatException("ID can't be negetive number");
        if (Boworker.Name == "")
            throw new FormatException("you must enter a name");
        if (Boworker.HourPrice <= 0)
            throw new FormatException("hour price can't be negetive number");
        if (Boworker.Email == "")
            throw new FormatException("you must enter an email");

        Do.Worker Doworker = new Do.Worker(Boworker.Id, (Do.Rank)Boworker.WorkerRank, Boworker.HourPrice, Boworker.Name, Boworker.Email);
        try
        {
            int idWorker = _dal.Worker.Create(Doworker);
            return idWorker;
        }
        catch (Do.DalAlreadyExistsException ex)
        {
            throw new BlAlreadyExistsException($"worker with ID={Boworker.Id} already exists", ex);

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
                if (getStatus(doTask) == BO.Status.OnTrackStarted || getStatus(doTask) == BO.Status.Done)
                    throw new Exception($"this worker with ID={_Id} is in the middle of task, or has finished it, so he can't be deleted");
                _dal.Task.Update(doTask with { IdWorker = null });
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
    }

    public static BO.Status getStatus(Do.Task task)
    {
        var dateTimeNow = DateTime.Now;
        
        return task switch
        {
            Do.Task t when t.IdWorker is null => BO.Status.Unscheduled,
            Do.Task t when t.StartDate > dateTimeNow => BO.Status.Scheduled,
            Do.Task t when  t.EndingDate > dateTimeNow => BO.Status.OnTrackStarted,
            _ => BO.Status.Done,
        };
    }

    public BO.Worker? Read(int id)
    {
        Do.Worker doWorker = _dal.Worker.Read(id)!;

        if (doWorker == null)
            throw new BlDoesNotExistException($"worker with ID={id} doesnt exists");

      
        var task = _dal.Task.Read(tmpTask => tmpTask.IdWorker == id && getStatus(tmpTask) is Status.OnTrackStarted) ;

        return new BO.Worker()
        {
            Id = id,
            WorkerRank = (BO.Rank)doWorker.WorkerRank,
            HourPrice = doWorker.HourPrice,
            Name = doWorker.Name,
            Email = doWorker.Email,
            WorkerTask = new WorkerTask
            {
                Id = task!.Id,
                Name = task.Name,
            }
        };
    }

    public IEnumerable<BO.Worker> ReadAll(Func<BO.Worker, bool>? filter = null)
    {
         var result = (from Do.Worker doWorker in ReadAll(filter)
                      let task = _dal.Task.Read(tmp => tmp.IdWorker == doWorker.Id)
                      select new BO.Worker()
                      {
                          Id = doWorker.Id,
                          WorkerRank = (BO.Rank)doWorker.WorkerRank,
                          HourPrice = doWorker.HourPrice,
                          Name = doWorker.Name,
                          Email = doWorker.Email,
                          WorkerTask = new WorkerTask
                          {
                              Id = task.Id,
                              Name = task.Name,
                          }
                      } ) ;
        return result;
    }

    public void Update(BO.Worker boWorker)
    {
        BO.Worker oldWorker = Read(boWorker.Id)!;

        if (boWorker.Id <= 0)
            throw new FormatException("ID can't be negetive number");

        if (boWorker.Name == "")
            throw new FormatException("you must enter a name");

        if (boWorker.HourPrice <= 0)
            throw new FormatException("hour price can't be negetive number");

        if (boWorker.Email == "")
            throw new FormatException("you must enter an email");

        if (oldWorker.WorkerRank > boWorker.WorkerRank)
            throw new FormatException("worker rank can only increase");

        if (boWorker.WorkerTask.Id is int taskId)
        {
            var doTask = _dal.Task.Read(taskId);
            if(doTask is not null)
            {
                doTask = doTask with { IdWorker = boWorker.Id };
                _dal.Task.Update(doTask);
            }
        } 
        Do.Worker Doworker = new Do.Worker(boWorker.Id, (Do.Rank)boWorker.WorkerRank, boWorker.HourPrice,
            boWorker.Name, boWorker.Email);
        try
        {
            _dal.Worker.Update(Doworker);
        }
        catch (Do.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"worker with ID={Doworker.Id} doesnt exists", ex);
        }
    }



}
