

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

    public DateTime StartDateProject =>  new DateTime();

    public DateTime EndDateProject => new DateTime();

    public void AutometicSchedule()
    {
        var tasks = BlApi.Factory.Get().Task;

        foreach(var task in tasks.ReadAll())
        {
            if(task.DependenceTasks == null)
                task.StartDate = StartDateProject;
            else
            {
                var max = tasks.Read(task.Id)!.DeadLine;
                foreach (var taskList in task.DependenceTasks)
                {
                    if (tasks.Read(taskList.Id)!.DeadLine > max)
                        max = tasks.Read(taskList.Id)!.DeadLine;
                }
                task.StartDate = max;
            }
        }
    }

    public StatusProject CheckStatusProject()//check
    {
        var tasks = BlApi.Factory.Get().Task.ReadAll();
        var NoStartDate = from BO.Task boTask in tasks
                          where boTask.StartDate == null
                          select boTask;
        if (NoStartDate.Count() > 0)
            return StatusProject.Planning;
        var noWantedStartDate = from BO.Task boTask in tasks
                          where boTask.WantedStartDate == null
                          select boTask;
        if (noWantedStartDate.Count() > 0)
            return StatusProject.Mid;
        return StatusProject.Execution; 

    }
}
