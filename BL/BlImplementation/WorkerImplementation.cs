namespace BlImplementation;
using BlApi;
using BO;
using System.Security.Cryptography;

internal class WorkerImplementation : IWorker
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Worker Boworker)
    {
       
        Do.Worker Doworker = new Do.Worker(Boworker.Id, (Do.Rank)Boworker.WorkerRank, Boworker.HourPrice, Boworker.Name, Boworker.Email);
        try
        {
            int idWorker = _dal.Worker.Create(Doworker);
            return idWorker;
        }
        catch(Do.DalAlreadyExistsException ex)
        {
            throw new Exception($"student withe ID={Boworker.Id} already exists",ex);
            
        }
    }

    public void Delete(int _Id)
    {
           
        try
        {
            _dal.Worker.Delete(_Id);
        }
        catch(Do.DalAlreadyExistsException ex)
        {
            throw new Exception($"Worker with ID={_Id} doesnt exists",ex);
        }

    }

    public IEnumerable<BO.TaskList> GetTask(int WorkerId)
    {
        BO.Worker Boworker = Read(WorkerId)!;
        if (Boworker.Tasks == null)
            throw new Exception($"this worker with ID={WorkerId} has no task");
        return Boworker.Tasks;

    }

    public BO.Worker? Read(int _Id)
    {
        Do.Worker Doworker=_dal.Worker.Read(_Id)!;
        if (Doworker == null)
            throw new Exception($"student withe ID={_Id} doesnt exists");
        return new BO.Worker()
        { Id = _Id, WorkerRank = (BO.Rank)Doworker.WorkerRank, HourPrice = Doworker.HourPrice, Name = Doworker.Name, Email = Doworker.Email };
    }

   

    public IEnumerable<BO.WorkerTask> ReadAll(Func<BO.Worker, bool>? filter = null)
    {
        return (from Do.Worker Doworker in _dal.Worker.ReadAll((Func<Do.Worker, bool>?)filter)
                select new BO.WorkerTask { Id = Doworker.Id, Name = Doworker.Name }
               );

    }


    public void Update(BO.Worker Boworker)
    {
        Do.Worker Doworker = new Do.Worker(Boworker.Id, (Do.Rank)Boworker.WorkerRank, Boworker.HourPrice, Boworker.Name, Boworker.Email);
        try
        {

            _dal.Worker.Update(Doworker);

        }
        catch (Do.DalDoesNotExistException ex )
        {
            throw new Exception ($"student withe ID={Doworker.Id} doesnt exists",ex);
        }
    }

 
  
}
