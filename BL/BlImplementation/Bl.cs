

using BlApi;
using BO;
using System.Security.Cryptography;

namespace BlImplementation;

internal class Bl : IBl
{
    public DalApi.IDal _dal = DalApi.Factory.Get;

    public ITask Task => new TaskImplementation();

    public ITaskList TaskList => new TaskListImplementation();

    public IWorker Worker => new WorkerImplementation();

    public IWorkerTask WorkerTask => new WorkerTaskImplementation();

    //public DateTime StartDateProject => new();

   // public DateTime EndDateProject => new();

    public void AutometicSchedule()
    {
        var tasks = BlApi.Factory.Get().Task;


        foreach (var task in tasks.ReadAll())
        {
            BO.Task wantedTask = tasks.Read(task.Id)!;
            if (wantedTask.DependenceTasks == null)
                wantedTask.StartDate = _dal.GetStartDate();
            else
            {
                var max = tasks.Read(task.Id)!.DeadLine;
                foreach (var taskList in wantedTask.DependenceTasks)
                {
                    if (tasks.Read(taskList.Id)!.DeadLine > max)
                        max = tasks.Read(taskList.Id)!.DeadLine;
                }
                wantedTask.StartDate = max;
            }
        }
    }

    public StatusProject CheckStatusProject()//check
    {
        var tasks = BlApi.Factory.Get().Task.ReadAll();
        if (BlApi.Factory.Get().getStartProject() == null)
            return StatusProject.Planning;
        var noStartDate = from BO.TaskList boTask in tasks
                          let task = BlApi.Factory.Get().Task.Read(boTask.Id)
                          where task.StartDate == null ||task.IdWorker==null
                          select boTask;
        if (noStartDate.Count() > 0)
            return StatusProject.Mid;
        return StatusProject.Execution; 

    }

    public DateTime? getEndProject()=> _dal.GetEndDate();
    public DateTime? getStartProject() => _dal.GetStartDate();  
    public void setEndProject(DateTime? date)=> _dal.SetEndDate(date);
    public void setStartProject(DateTime? date)=>_dal.SetStartDate(date);
   
}
