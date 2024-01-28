namespace BlImplementation;
using BlApi;
using BO;

internal class WorkerTaskImplementation : IWorkerTask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public WorkerTask GetWorkerTask(int IdTask)
    {
        throw new NotImplementedException();
    }
}
