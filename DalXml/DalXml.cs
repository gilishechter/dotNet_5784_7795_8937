using DalApi;
using System.Diagnostics;

namespace Dal;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    public IWorker Worker => new WorkerImplementation();
    public ITask Task => new TaskImplementation();
    public IDependency Dependency => new DependencyImplementation();

 
}

