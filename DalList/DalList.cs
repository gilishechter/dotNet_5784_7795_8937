namespace Dal;
using DalApi;
/// <summary>
/// create object of the interfaces typs
/// </summary>
sealed public class DalList : IDal
{
    public IWorker Worker => new WorkerImplementation();
    public IDependency Dependency => new DependenceImplementation();
    public ITask Task => new TaskImplementation();

}
