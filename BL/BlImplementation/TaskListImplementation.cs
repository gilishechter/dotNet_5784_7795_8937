namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

internal class TaskListImplementation : ITaskList
{
    private DalApi.IDal _dal = DalApi.Factory.Get;


    private readonly Bl _bl;
    internal TaskListImplementation(Bl bl) => _bl = bl;

    public BO.Status GetStatus(Do.Task task)
    {

        //If the task does not have the id of the worker working on it, the status is Unscheduled
        //If the task have the id of the worker working on it,and the start date don't come yet the status is Scheduled
        //If the task have the id of the worker working on it,and the start date come but the end date dont come yet status is Scheduled OnTrackStarted
        //else done

        return task switch
        {
            Do.Task t when t.IdWorker is null => BO.Status.Unscheduled,
            Do.Task t when t.StartDate > _bl.Clock => BO.Status.Scheduled,
            Do.Task t when t.DeadLine > _bl.Clock => BO.Status.OnTrackStarted,
            _ => BO.Status.Done,
        };
    }
    public IEnumerable<BO.TaskList> getDependenceList(Do.Task doTask)
    {
        var result = (from Do.Dependency dependency in _dal.Dependency.ReadAll()
                      where doTask.Id == dependency.DependenceTask && _dal.Task.Read(dependency.PrevTask) != null

                      select new BO.TaskList//create the current task list for each dependence
                      {
                          Id = dependency.PrevTask,
                          Name = _dal.Task.Read(dependency.PrevTask)!.Name,
                          Description = _dal.Task.Read(dependency.PrevTask)!.Description,
                          Status = GetStatus(doTask)

                      });
        return result;
    }
    public int Create(int IdCurrentTask,int idtaskList)
    {
        if (IdCurrentTask == idtaskList)
            throw new BlAlreadyExistsException("A task can't be depend on it self");

        Do.Task task = _dal.Task.Read(IdCurrentTask);
        var tasks = getDependenceList(task).Where(t => t.Id == idtaskList);
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
                        Status =GetStatus(doTask),
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
                        Status =GetStatus(doTask),
                    }
                    where filter(boTask)
                    select boTask
                    );
        }
    }
}
