namespace BlApi;

public interface IWorker
{
    public IEnumerable<BO.WorkerTask> ReadAll(Func<BO.Worker, bool>? filter = null);
    public void Update(BO.Worker worker);
    public void Delete(int Id);
    public int Create(BO.Worker worker);
    public BO.Worker? Read(int Id);
    public IEnumerable<BO.TaskList> GetTask(int WorkerId);//return the task that the worker works on

}
