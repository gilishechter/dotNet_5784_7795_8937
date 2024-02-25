namespace DalApi;

/// <summary>
/// interface that include the other 3 interfaces
/// </summary>
public interface IDal
{
    IWorker Worker { get; }
    IDependency Dependency { get; }
    ITask Task { get; }
    IUser User { get; }

    //public DateTime? StartProjectDate { get; set; }
    //public DateTime? EndProjectDate { get; set; }
    public void SetStartDate(DateTime? startDate);
    public DateTime? GetStartDate();
    public void SetEndDate(DateTime? startDate);
    public DateTime? GetEndDate();


}

