namespace DalApi;

/// <summary>
/// interface that include the other 3 interfaces
/// </summary>
public interface IDal
{
    IWorker Worker { get; }
    IDependency Dependency { get; }
    ITask Task { get; }

    public void setStartDate(DateTime? startDate);
    public DateTime? getStartDate();
    public void setEndDate(DateTime? startDate);
    public DateTime? getEndDate();
}

