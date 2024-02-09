

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

    //public static DateTime? StartDateProject { get; set; } = new DateTime(2024,1,1);
    //public static DateTime? EndDateProject { get => 

    public void setStartProject(DateTime? date);
    public void setEndProject(DateTime? date);
    public DateTime? getStartProject();
    public DateTime? getEndProject();
    public StatusProject CheckStatusProject();
    //public void AutometicSchedule(); -we didn't do the bonus yet
}
