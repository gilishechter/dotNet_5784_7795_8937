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


    public void SetStartDate(DateTime? startDate) => DataSource.Config.StartProjectDate = startDate;
    public DateTime? GetStartDate() { return DataSource.Config.StartProjectDate; } 
    public void SetEndDate(DateTime? endDate) => DataSource.Config.endProjectDate = endDate;  
    public DateTime? GetEndDate()  { return DataSource.Config.endProjectDate; }

   
}

