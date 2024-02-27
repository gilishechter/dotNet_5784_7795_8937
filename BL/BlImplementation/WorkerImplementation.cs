namespace BlImplementation;
using BlApi;
using BO;

internal class WorkerImplementation : IWorker
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    private readonly Bl _bl;
    internal WorkerImplementation(Bl bl) => _bl = bl;

    /// <summary>
    /// The function get bo worker and throw exception / create the worker object
    /// </summary>
    /// <param name="boWorker"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="BlAlreadyExistsException"></exception>
    public int Create(BO.Worker boWorker)
    {
        if (boWorker.Id <= 0)
            throw new FormatException("Incorrect ID");
        if (boWorker.Id == 0)
            throw new FormatException("you must enter an ID");
        if (boWorker.Name == null)
            throw new FormatException("you must enter a name");
        if (boWorker.HourPrice <= 0)
            throw new FormatException("Incorrect hour price");
        if ((boWorker.Email == null))
            throw new FormatException("you must enter an email");
        if ((!boWorker.Email!.Contains("@gmail.com")))
            throw new FormatException("the email is invalid");

        Do.Worker Doworker = new(boWorker.Id, (Do.Rank)boWorker.WorkerRank, boWorker.HourPrice, boWorker.Name, boWorker.Email);
        boWorker.WorkerTask = GetWorkerTask(Doworker);//update current task for worker 
        try
        {
            int idWorker = _dal.Worker.Create(Doworker);//try to create the worker and catch  
            return idWorker;
        }
        catch (Do.DalAlreadyExistsException ex)
        {
            throw new BlAlreadyExistsException($"worker with ID={boWorker.Id} already exists", ex);

        }
    }
    /// <summary>
    /// the function delete the worker withe the id thar the function get and throw match exeption
    /// </summary>
    /// <param name="_Id"></param>
    /// <exception cref="BlCantBeDeleted"></exception>
    /// <exception cref="BlDuringExecution"></exception>
    /// <exception cref="BlDoesNotExistException"></exception>
    public void Delete(int _Id)
    {
        BO.Worker boWorker = Read(_Id)!;

        if (boWorker.WorkerTask!.Id is int taskId)
        {
            var doTask = _dal.Task.Read(taskId);
            if (doTask is not null)
            {
                if (GetStatus(doTask) == BO.Status.OnTrackStarted || GetStatus(doTask) == BO.Status.Done)
                    throw new BlCantBeDeleted($"this worker with ID={_Id} is in the middle of task, or has finished it, so he can't be deleted");//you can't delete worker if he work on task
                _dal.Task.Update(doTask with { IdWorker = null });
                if (BlApi.Factory.Get().CheckStatusProject() == BO.StatusProject.Execution && GetStatus(doTask) == BO.Status.Scheduled)
                    throw new BlDuringExecution("you cant delete worker during the execution if he has a task.");
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
    /// <summary>
    /// the function return the match status of the task 
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    public  BO.Status GetStatus(Do.Task task)
    {
       
        //If the task does not have the id of the worker working on it, the status is Unscheduled
        //If the task have the id of the worker working on it,and the start date don't come yet the status is Scheduled
        //If the task have the id of the worker working on it,and the start date come but the end date dont come yet status is Scheduled OnTrackStarted
        //else done

        return task switch
        {
            Do.Task t when t.IdWorker is null => BO.Status.Unscheduled,
            Do.Task t when t.StartDate > _bl.Clock => BO.Status.Scheduled,
            Do.Task t when t.DeadLine > _bl.Clock => BO.Status.OnTrackStarted,
            _ => BO.Status.Done,
        };
    }

    /// <summary>
    /// the function return the worker with the id that the function get
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
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
    /// <summary>
    /// return all the worker
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.Worker> ReadAll(Func<BO.Worker, bool>? filter = null)
    {
        IEnumerable<BO.Worker> result;
        if (filter == null)
        {
            result = (from Do.Worker doWorker in _dal.Worker.ReadAll()
                      let task = _dal.Task.Read(tmp => tmp.IdWorker == doWorker.Id && GetStatus(tmp) is Status.OnTrackStarted)
                      select new BO.Worker()
                      {
                          Id = doWorker.Id,
                          WorkerRank = (BO.Rank)doWorker.WorkerRank,
                          HourPrice = doWorker.HourPrice,
                          Name = doWorker.Name,
                          Email = doWorker.Email,
                          WorkerTask = GetWorkerTask(doWorker)
                      });
        }
        else
        {
            result = (from Do.Worker doWorker in _dal.Worker.ReadAll()
                      let task = _dal.Task.Read(tmp => tmp.IdWorker == doWorker.Id && GetStatus(tmp) is Status.OnTrackStarted)
                      let boWorker = new BO.Worker()
                      {
                          Id = doWorker.Id,
                          WorkerRank = (BO.Rank)doWorker.WorkerRank,
                          HourPrice = doWorker.HourPrice,
                          Name = doWorker.Name,
                          Email = doWorker.Email,

                          WorkerTask = GetWorkerTask(doWorker)
                      }
                      where filter(boWorker)
                      select boWorker
                      );
        }
        var orderResult = result.OrderBy(doWorker => doWorker.Name); //order the worker by name
        return orderResult;
    }

    // the function update the old worker to the new worker that the worke get
    public void Update(BO.Worker boWorker)
    {
        BO.Worker oldWorker = Read(boWorker.Id)!;
        //Checks if illogical values ​​have been entered
        if (boWorker.HourPrice <= 0)
            throw new FormatException("hour price can't be negetive number");

        if ((boWorker.Email != "") && (!boWorker.Email!.Contains("@gmail.com")))
            throw new FormatException("this email is wrong please enter a valid email");

        if (oldWorker.WorkerRank > boWorker.WorkerRank)
            throw new FormatException("worker rank can only increase");

        if (boWorker.WorkerTask != null && boWorker.WorkerTask.Id is int taskId)//put the int value in taskId because read get int?
        {
            var doTask = _dal.Task.Read(taskId);
            if (doTask is not null)
            {
                doTask = doTask with { IdWorker = boWorker.Id };
                _dal.Task.Update(doTask);
            }
        }
        Do.Worker Doworker = new(boWorker.Id, (Do.Rank)boWorker.WorkerRank, boWorker.HourPrice,
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

    /// <summary>
    /// The function sort the worker into grouping according to their rank and returns the group with the rank as the function receives
    /// </summary>
    /// <param name="rank"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IEnumerable<BO.Worker> RankGroup(int rank)
    {
        var groupWorkers = _dal.Worker.ReadAll().GroupBy(r => (int)r.WorkerRank);
        var wantedGroup = groupWorkers.FirstOrDefault(g => g.Key == rank);
        if (wantedGroup == null)
            throw new Exception("there is no such a worker");
        return from Do.Worker doWorker in wantedGroup
               select new BO.Worker()
               {
                   Id = doWorker.Id,
                   WorkerRank = (BO.Rank)doWorker.WorkerRank,
                   HourPrice = doWorker.HourPrice,
                   Name = doWorker.Name,
                   Email = doWorker.Email,
                   WorkerTask = GetWorkerTask(doWorker)
               };
    }
    /// <summary>
    /// the function update current task for worker
    /// </summary>
    /// <param name="doWorker"></param>
    /// <returns></returns>
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
