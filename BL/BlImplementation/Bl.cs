using BlApi;
using BO;
//using DalApi;
using System.Linq;
using System.Security.Cryptography;

namespace BlImplementation;

internal class Bl : IBl
{
    public DalApi.IDal _dal = DalApi.Factory.Get;

    public ITask Task => new TaskImplementation(this);

    public ITaskList TaskList => new TaskListImplementation(this);

    public IWorker Worker => new WorkerImplementation(this);
    
    public IWorkerTask WorkerTask => new WorkerTaskImplementation();

    public IUser User =>  new UserImplementation();
    public IEnumerable<BO.TaskList> getDependenceList(Do.Task doTask)
    {
        var result = (from Do.Dependency dependency in _dal.Dependency.ReadAll()
                      where doTask.Id == dependency.DependenceTask && _dal.Task.Read(dependency.PrevTask) != null

                      select new BO.TaskList//create the current task list for each dependence
                      {
                          Id = dependency.PrevTask,
                          Name = _dal.Task.Read(dependency.PrevTask)!.Name,
                          Description = _dal.Task.Read(dependency.PrevTask)!.Description,
                          Status = GetStatus(_dal.Task.Read(dependency.PrevTask))

                      });
        return result;
    }

    public BO.Status GetStatus(Do.Task task)
    {

        //If the task does not have the id of the worker working on it, the status is Unscheduled
        //If the task have the id of the worker working on it,and the start date don't come yet the status is Scheduled
        //If the task have the id of the worker working on it,and the start date come but the end date dont come yet status is Scheduled OnTrackStarted
        //else done

        return task switch
        {
            Do.Task t when t.DeadLine < _bl.Clock && t.EndingDate == null => BO.Status.InJeopardy,
            Do.Task t when t.IdWorker is null || t.IdWorker is 0 => BO.Status.Unscheduled,
            Do.Task t when t.StartDate > _bl.Clock || t.StartDate == null => BO.Status.Scheduled,
            Do.Task t when t.DeadLine > _bl.Clock => BO.Status.OnTrackStarted,
            _ => BO.Status.Done,
        };
    }
    private readonly IBl _bl;
  
    public StatusProject CheckStatusProject()
    {
        var tasks = BlApi.Factory.Get().Task.ReadAll();
        if (BlApi.Factory.Get().GetStartProject() == null)
            return StatusProject.Planning;
        var noStartDate = from BO.TaskList boTask in tasks
                          let task = BlApi.Factory.Get().Task.Read(boTask.Id)
                          where task.StartDate == null ||task.IdWorker==null
                          select boTask;
        if (noStartDate.Count() > 0)
            return StatusProject.Mid;
        return StatusProject.Execution; 

    }

    public void Clear()
    {
        DalTest.Initialization.Clear();
    }

    public DateTime? GetEndProject()=> _dal.GetEndDate();
    public DateTime? GetStartProject() => _dal.GetStartDate();  
    public void SetEndProject(DateTime? date)=> _dal.SetEndDate(date);
    public void SetStartProject(DateTime? date) => _dal.SetStartDate(date);

    public DateTime SetClocMonth()
    {
        return Clock = Clock.AddMonths(1);
    }

    public DateTime SetClockYear()
    {
        return Clock=Clock.AddYears(1);
    }

    public DateTime SetClockDay()
    {
        return Clock=Clock.AddDays(1);
    }

    public DateTime ResetClock()
    {
        return Clock = DateTime.Now;
    }

    private static DateTime s_Clock = DateTime.Now.Date;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

}
