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
            Do.Task t when t.IdWorker is null || t.IdWorker is 0 => BO.Status.Unscheduled,
            Do.Task t when t.StartDate== null || t.StartDate > _bl.Clock => BO.Status.Scheduled,
            Do.Task t when t.DeadLine > _bl.Clock => BO.Status.OnTrackStarted,
            _ => BO.Status.Done,
        };
    }
    private readonly IBl _bl;
    //internal BlImplementation(IBl bl) => _bl = bl;

    //public DateTime? CreateSchedule(int _id, DateTime? _date)
    //{
    //    Do.Task? _task = _dal.Task.Read(_id);
    //    if (_task == null)//if the task doesn't exist
    //        throw new BlDoesNotExistException($"this task with id ={_id} doesn't exist");

    //    var startDates = from BO.TaskList taskList in getDependenceList(_task)
    //                     where getDependenceList(_task) != null && _dal.Task.Read(taskList.Id) != null && _dal.Task.Read(taskList.Id)!.WantedStartDate == null
    //                     select taskList;
    //    if (startDates.Count() > 0)//if the previous tasks don't have start dates
    //        throw new BlCantUpdateStartDateExecution("You can't update the start date, because the previous tasks don't have start dates");
        
    //    var endDates = from BO.TaskList taskList in getDependenceList(_task)
    //                   where getDependenceList(_task) != null && _dal.Task.Read(taskList.Id) != null && _dal.Task.Read(taskList.Id)!.EndingDate > _date
    //                   select taskList;
    //    if (endDates.Count() > 0)//if the date is sooner then the previous tasks end dates
    //        throw new BlCantUpdateStartDateExecution("You can't update the start date, because the date is sooner then the previous tasks end dates");

    //    if (getDependenceList(_task) == null && _date < _dal.GetStartDate())//if he date is sooner then the start project date
    //        throw new BlCantUpdateStartDateExecution("You can't update the start date because the date is sooner then the start project date");


    //    if (_task.WantedStartDate != null && _date < _task.WantedStartDate)
    //        throw new BlCantUpdateStartDateExecution("You can't update the start date because the planned start date didn't arrive yet");

    //    Do.Task newTask = _task with { WantedStartDate = _date };
    //    _dal.Task.Update(newTask);
    //    return _date;

    //}

    //public void AutometicSchedule()
    //{
    //    List<BO.TaskList> allNoStartDate = new List<BO.TaskList>();
    //    IEnumerable<Do.Task> allTasks = _dal.Task.ReadAll();

    //    foreach (var task in allTasks)
    //    {
    //        if ((getDependenceList(task)).Count() == 0)
    //        {
    //            if(task.WantedStartDate == null)
    //                CreateSchedule(task.Id, _dal.GetStartDate());
    //        }
    //    }
    //    allTasks = _dal.Task.ReadAll();
    //    while (allTasks.Any(t => t.WantedStartDate == null))
    //    {
    //        foreach (Do.Task task in allTasks)
    //        {
    //            IEnumerable<BO.TaskList> tasksDependencies = getDependenceList(task);
    //            List<BO.TaskList> noStartDate = new List<BO.TaskList>();
    //            List<Do.Task> thereIsStartDate = new List<Do.Task>();

    //            foreach (BO.TaskList taskList in tasksDependencies)
    //            {
    //                Do.Task dep = _dal.Task.Read(taskList.Id);
    //                if (dep.WantedStartDate == null)
    //                {
    //                    noStartDate.Add(taskList);
    //                    allNoStartDate.Add(taskList);
    //                }
    //                else
    //                {
    //                    thereIsStartDate.Add(dep);
    //                }
    //            }
    //            if (noStartDate.Count() == 0 && task.WantedStartDate== null)
    //            {
    //                DateTime? scheduledDate;
    //               // if (task.EndingDate == null)
    //                    scheduledDate = thereIsStartDate.Max(dep => dep.WantedStartDate + dep.Time);
    //              //  else
    //                  //  scheduledDate = thereIsStartDate.Max(dep => dep. + dep.Time);

    //                CreateSchedule(task.Id, (DateTime)scheduledDate);
    //                allTasks = _dal.Task.ReadAll();
    //            }
    //        }           
    //    }
    //}

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

    public DateTime SetClockHour()
    {
        return Clock = Clock.AddHours(1);
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

    private static DateTime s_Clock = DateTime.Now;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

}
