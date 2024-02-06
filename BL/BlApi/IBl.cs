

using BO;

namespace BlApi;

public interface IBl
{
    public ITask Task { get;}
    public ITaskList TaskList { get;}
    public IWorker Worker { get;}
    public IWorkerTask WorkerTask { get;}

    public static DateTime? StartDateProject { get; set; } =new DateTime(2024,1,1);
    public static DateTime? EndDateProject { get; set; } = null;

    public StatusProject CheckStatusProject();
    public void AutometicSchedule();
}
