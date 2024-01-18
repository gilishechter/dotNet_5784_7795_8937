namespace DalApi;

/// <summary>
/// interface that include the other 3 interfaces
/// </summary>
public interface IDal
{
    IWorker Worker { get; }
    IDependency Dependencies { get; }
    ITask Task { get; }
}

