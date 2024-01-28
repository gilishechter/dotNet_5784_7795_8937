namespace BlApi;

public interface IWorker
{
    public IEnumerable<BO.Worker> ReadAll(Func<BO.Worker, bool>? filter = null);
    public void Update(BO.Worker worker);
    public void Delete(int Id);
    public int Create(BO.Worker worker);
    public BO.Worker? Read(int Id);
    //public BO.TaskList GetTask(int WorkerId);//return the task that the worker works on

}
