namespace BlApi;

public interface ITask
{
    public IEnumerable<BO.Task> ReadAll(Func<Task, bool>? filter = null);
    public void Update(BO.Task task);
    public void Delete(int Id);
    public int Create(BO.Task task);
    public BO.Task? Read(int Id);
}
