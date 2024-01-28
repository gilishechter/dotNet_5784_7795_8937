namespace BlImplementation;
using BlApi;
using BO;
using System.Collections.Generic;

internal class TaskListImplementation : ITaskList
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public IEnumerable<TaskList> GetTaskListsTheWorkerCanDo(int workerId)
    {
        throw new NotImplementedException();
    }
}
