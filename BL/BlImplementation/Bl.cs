

using BlApi;
using BO;

namespace BlImplementation;

internal class Bl : IBl
{
    public ITask Task => new TaskImplementation();

    public ITaskList TaskList => new TaskListImplementation();

    public IWorker Worker => new WorkerImplementation();

    public IWorkerTask WorkerTask => new WorkerTaskImplementation();
}
