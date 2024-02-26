namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

internal class TaskListImplementation : ITaskList
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(int IdCurrentTask,int idtaskList)
    {
        if (IdCurrentTask == idtaskList)
            throw new BlAlreadyExistsException("A task can't be depend on it self");

        Do.Task task = _dal.Task.Read(IdCurrentTask);
        var tasks = TaskImplementation.getDependenceList(task).Where(t => t.Id == idtaskList);
        if (tasks.Count() != 0)
            throw new BlAlreadyExistsException("This task already depends on the task you selected");

        _dal.Dependency.Create(new Do.Dependency(0, IdCurrentTask, idtaskList));
        return idtaskList;
    }

    public void Delete(int IdCurrentTask, int IdPrevTask)
    {
        var result = _dal.Dependency.ReadAll().FirstOrDefault(dep => IdPrevTask == dep.PrevTask && IdCurrentTask == dep.DependenceTask);
        _dal.Dependency.Delete(result.Id);

    }

    public IEnumerable<TaskList> ReadAll(Func<TaskList, bool>? filter = null)
    {
        if (filter == null)//if there is no filter , create task list for each task
        {
            return (from Do.Task doTask in _dal.Task.ReadAll()
                    select new BO.TaskList()
                    {
                        Id = doTask.Id,
                        Name = doTask.Name,
                        Description = doTask.Description,
                        Status = WorkerImplementation.GetStatus(doTask),
                    });
        }
        else
        {//ifthere is filter create the task list but choose only the tasks that the filter match them
            return (from Do.Task doTask in _dal.Task.ReadAll()
                    let boTask = new BO.TaskList()
                    {
                        Id = doTask.Id,
                        Name = doTask.Name,
                        Description = doTask.Description,
                        Status = WorkerImplementation.GetStatus(doTask),
                    }
                    where filter(boTask)
                    select boTask
                    );
        }
    }
}
