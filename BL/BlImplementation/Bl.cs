

using BlApi;
using BO;

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
        throw new NotImplementedException();
    }

    public StatusProject CheckStatusProject()//check
    {
        var tasks = BlApi.Factory.Get().Task.ReadAll(task => task.)
        var NoStartDate = from BO.Task boTask in tasks
                          
    }
}
