namespace Dal;
using DalApi;
/// <summary>
/// create object of the interfaces typs
/// </summary>
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }

    public IWorker Worker => new WorkerImplementation();
    public IDependency Dependency => new DependenceImplementation();
    public ITask Task => new TaskImplementation();

    public void setStartDate(DateTime? startDate) => DataSource.Config.startDate = startDate;
    public DateTime? getStartDate() { return DataSource.Config.startDate; }
    public void setEndDate(DateTime? endDate) => DataSource.Config.startDate = endDate;  
    public DateTime? getEndDate()   { return DataSource.Config.endDate; }

}
