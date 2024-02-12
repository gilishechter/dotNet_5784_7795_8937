

using BO;
using DalApi;
using System.Reflection.Metadata.Ecma335;
namespace BlApi;

public interface IBl
{

    public ITask Task { get;}
    public ITaskList TaskList { get;}
    public IWorker Worker { get;}
    public IWorkerTask WorkerTask { get;}

    public void SetStartProject(DateTime? date);
    public void SetEndProject(DateTime? date);
    public DateTime? GetStartProject();
    public DateTime? GetEndProject();
    public StatusProject CheckStatusProject();
    //public void AutometicSchedule(); we didn't do the bonus yet
}
