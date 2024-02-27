

using BlApi;
using BO;
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
   


    //public void AutometicSchedule()            //we didn't do the bonus yet
    //{
    //    var tasks = BlApi.Factory.Get().Task;

    //    foreach (var task in tasks.ReadAll())
    //    {
    //        var fullTask = tasks.Read(task.Id);
    //        if (fullTask.DependenceTasks == null)
    //            fullTask.StartDate = _dal.GetStartDate();
    //    }
    //}

    //    foreach (var task in tasks.ReadAll())
    //    {
    //        BO.Task wantedTask = tasks.Read(task.Id)!;
    //        if (wantedTask.DependenceTasks == null)
    //            wantedTask.StartDate = _dal.GetStartDate();
    //        else
    //        {
    //            var max = tasks.Read(task.Id)!.DeadLine;
    //            foreach (var taskList in wantedTask.DependenceTasks)
    //            {
    //                if (tasks.Read(taskList.Id)!.DeadLine > max)
    //                    max = tasks.Read(taskList.Id)!.DeadLine;
    //            }
    //            wantedTask.StartDate = max;
    //        }
    //    }
    //}

    //public void recursiveAutoSchedule(BO.Task task)
    //{
    //    var tasks = BlApi.Factory.Get().Task;
    //    // var fullTask = tasks.Read(task.Id);
    //    var NostartDates = task.DependenceTasks.Where(date => tasks.Read(date.Id).StartDate == null);
    //    if(NostartDates.Count() == 0 )
    //    {
    //        //var startDates = task.DependenceTasks.Where(date => tasks.Read(date.Id).StartDate != null);
    //        foreach(var x)

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
