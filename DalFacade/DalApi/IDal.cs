namespace DalApi;
public interface IDal
{
    IWorker Worker { get; }
    IDependence Dependencies { get; }
    ITask Task { get; }
}

