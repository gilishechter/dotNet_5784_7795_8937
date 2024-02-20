namespace BlApi;
public interface ITaskList
{
    public void Delete(int IdCurrentTask, int IdPrevTask);
    public int Create(int IdCurrentTask, int idtaskList);
    public IEnumerable<BO.TaskList> ReadAll(Func<BO.TaskList, bool>? filter = null);
}
