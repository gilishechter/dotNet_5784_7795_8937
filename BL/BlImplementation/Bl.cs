

using BlApi;
using BO;
using System.Security.Cryptography;

namespace BlImplementation;

internal class Bl : IBl
{
    public ITask Task => new TaskImplementation();

    public ITaskList TaskList => new TaskListImplementation();

    public IWorker Worker => new WorkerImplementation();

    public IWorkerTask WorkerTask => new WorkerTaskImplementation();

    public DateTime StartDateProject =>  new();

    public DateTime EndDateProject => new();

    public void AutometicSchedule()
    {
        var tasks = BlApi.Factory.Get().Task;

        //var task1 = from Do.Task doTask in tasks.ReadAll()
        //            let wantedTask = tasks.Read(doTask.Id)
        //            //from BO.TaskList taskList in wantedTask.DependenceTasks
        //            where wantedTask.DependenceTasks == null
        //            select wantedTask.StartDate = StartDateProject;

        //var task2 = from Do.Task doTask in tasks.ReadAll()
        //            let wantedTask = tasks.Read(doTask.Id)
        //            where wantedTask.DependenceTasks != null



        //            from BO.TaskList taskList in wantedTask.DependenceTasks
        //            let wantedDepTask = tasks.Read(taskList.Id)                   
        //            orderby wantedDepTask.StartDate descending
        //            select wantedTask.StartDate = wantedTask.DependenceTasks.FirstOrDefault()




        foreach (var task in tasks.ReadAll())
        {
            BO.Task wantedTask = tasks.Read(task.Id)!;
            if (wantedTask.DependenceTasks == null)
                wantedTask.StartDate = StartDateProject;
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
        //var NoStartDate = from BO.Task boTask in tasks
        //                  where boTask.StartDate == null
        //                  select boTask;
        if (IBl.StartDateProject == null)
            return StatusProject.Planning;
        var noStartDate = from BO.Task boTask in tasks
                          where boTask.StartDate == null
                          select boTask;
        if (noStartDate.Count() > 0)
            return StatusProject.Mid;
        return StatusProject.Execution; 

    }
}
