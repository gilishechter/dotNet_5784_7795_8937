namespace Dal;
using DalApi;
sealed public class DalList : IDal
{
    public IWorker Worker => new WorkerImplementation();
    public IDependence Dependencies => new DependenceImplementation();
    public ITask Task => new TaskImplementation();

}
