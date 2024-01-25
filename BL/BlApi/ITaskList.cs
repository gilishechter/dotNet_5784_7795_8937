namespace BlApi;
public interface ITaskList
{
    public IEnumerable<BO.TaskList> GetTaskListsTheWorkerCanDo(int workerId);//return the tasks the worker can do by his rank
}
