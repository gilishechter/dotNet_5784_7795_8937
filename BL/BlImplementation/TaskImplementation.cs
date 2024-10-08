﻿namespace BlImplementation;
using BlApi;
using DlhSoft.Windows.Data;
using System.Collections.ObjectModel;
using System.Runtime.Intrinsics.Arm;


internal class TaskImplementation : ITask
{
    public static DalApi.IDal _dal = DalApi.Factory.Get;

    private readonly IBl _bl;
    internal TaskImplementation(IBl bl) => _bl = bl;

    public void AutometicSchedule()
    {
        List<BO.TaskList> allNoStartDate = new List<BO.TaskList>();
        IEnumerable<Do.Task> allTasks = _dal.Task.ReadAll();

        foreach (var task in allTasks)
        {
            // task.WantedStartDate=null;
            Do.Task newTask = task with { WantedStartDate = null };
            _dal.Task.Update(newTask);
        }
        allTasks = _dal.Task.ReadAll();

        foreach (var task in allTasks)
        {
            if ((getDependenceList(task)).Count() == 0)
            {
                if (task.WantedStartDate == null)
                    CreateSchedule(task.Id, _dal.GetStartDate());
            }
        }
        
        allTasks = _dal.Task.ReadAll();
        while (allTasks.Any(t => t.WantedStartDate == null))
        {
            foreach (Do.Task task in allTasks)
            {
                IEnumerable<BO.TaskList> tasksDependencies = getDependenceList(task);
                List<BO.TaskList> noStartDate = new List<BO.TaskList>();
                List<Do.Task> thereIsStartDate = new List<Do.Task>();

                foreach (BO.TaskList taskList in tasksDependencies)
                {
                    Do.Task dep = _dal.Task.Read(taskList.Id);
                    if (dep.WantedStartDate == null)
                    {
                        noStartDate.Add(taskList);
                        allNoStartDate.Add(taskList);
                    }
                    else
                    {
                        thereIsStartDate.Add(dep);
                    }
                }
                if (noStartDate.Count() == 0 && task.WantedStartDate == null)
                {
                    DateTime? scheduledDate;
                    // if (task.EndingDate == null)
                    scheduledDate = thereIsStartDate.Max(dep => dep.WantedStartDate + dep.Time);
                    //  else
                    //  scheduledDate = thereIsStartDate.Max(dep => dep. + dep.Time);

                    CreateSchedule(task.Id, (DateTime)scheduledDate);
                    allTasks = _dal.Task.ReadAll();
                }
            }
        } 
        foreach(Do.Task task in _dal.Task.ReadAll())
        {
            Do.Task newTask = task with { DeadLine = task.WantedStartDate + task.Time };
            _dal.Task.Update(newTask);
        }
    }

    /// <summary>
    /// this function return the dependencies tasks
    /// </summary>
    /// <param name="doTask"></param>
    /// <returns></returns>

    public BO.Status GetStatus(Do.Task task)
    {

        //If the task does not have the id of the worker working on it, the status is Unscheduled
        //If the task have the id of the worker working on it,and the start date don't come yet the status is Scheduled
        //If the task have the id of the worker working on it,and the start date come but the end date dont come yet status is Scheduled OnTrackStarted
        //else done

        return task switch
        {
            Do.Task t when t.DeadLine < _bl.Clock && t.EndingDate == null => BO.Status.InJeopardy,
            Do.Task t when t.IdWorker is null || t.IdWorker is 0 => BO.Status.Unscheduled,
            Do.Task t when t.StartDate > _bl.Clock || t.StartDate == null => BO.Status.Scheduled,
            Do.Task t when t.DeadLine > _bl.Clock && t.EndingDate == null => BO.Status.OnTrackStarted,         
            _ => BO.Status.Done,
        };
    }

    public  IEnumerable<BO.TaskList> getDependenceList(Do.Task doTask)
    {
        var result = (from Do.Dependency dependency in _dal.Dependency.ReadAll()
                      where doTask.Id == dependency.DependenceTask && _dal.Task.Read(dependency.PrevTask) != null

                      select new BO.TaskList//create the current task list for each dependence
                      {
                          Id = dependency.PrevTask,
                          Name = _dal.Task.Read(dependency.PrevTask)!.Name,
                          Description = _dal.Task.Read(dependency.PrevTask)!.Description,
                          Status = GetStatus(_dal.Task.Read(dependency.PrevTask))

                      });
        return result;
    }
    public IEnumerable<BO.GanttDetails> GetDetailsToGantt(Func<BO.GanttDetails, bool>? filter = null)
    {
        return (from task in ReadAll()
                let t = _dal.Task.Read(task.Id)
                let start = (int)(t.WantedStartDate - _bl.GetStartProject()).Value.TotalHours
                select new BO.GanttDetails()
                {
                    ID = task.Id,
                    Name = task.Name,
                    TasksDays = (int)t.Time.Value.TotalHours * 2,
                    StartOffset = start * 2,
                    EndOffset = (int)(start + t.Time.Value.TotalHours) * 2,
                    Dependencies = StringDependencies(task.Id),
                    Status = task.Status,
                }).ToList().OrderBy(x => x.StartOffset);
    }

    private string StringDependencies(int id)
    {
        var x = _dal.Dependency.ReadAll(x => x.DependenceTask == id)
                                    .Where(dependency => dependency.DependenceTask == id)
                                    .Select(dependency => dependency.PrevTask.ToString() + " ");
        string dep = "";
        foreach (string tmp in x)
        {
            dep += tmp.ToString();
        }
        return dep;
    }

    /// <summary>
    /// create new BO object and send it to the DO create
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="BlWhilePlanning"></exception>
    /// <exception cref="BlDuringExecution"></exception>
    /// <exception cref="BlAlreadyExistsException"></exception>
    public int Create(BO.Task boTask)
    {
        if (BlApi.Factory.Get().CheckStatusProject() != BO.StatusProject.Planning)
            throw new BlCantBeUpdated("you can't create a a task since the project started");

        if (boTask.Id < 0)
            throw new FormatException("ID can't be negetive number");

        if (boTask.Name == null)
            throw new FormatException("you must enter a name");

        if (BlApi.Factory.Get().CheckStatusProject() == BO.StatusProject.Planning && boTask.IdWorker != null)//throw match exeptions
            throw new BlWhilePlanning("you cant assign a worker while planning the project");

        if (BlApi.Factory.Get().CheckStatusProject() == BO.StatusProject.Planning && boTask.WantedStartDate != null)
            throw new BlWhilePlanning("you cant update a wanted start date while planning the project");

        if (BlApi.Factory.Get().CheckStatusProject() == BO.StatusProject.Execution)
            throw new BlDuringExecution("yoe can'd add task during execution");

        if (boTask.Status == BO.Status.None)
            throw new BlCantCreate("you cant choose None as status");

        DateTime? DeadLine = boTask.StartDate > boTask.WantedStartDate ? boTask.StartDate + boTask.Time : boTask.WantedStartDate + boTask.Time;
        DateTime? createDate = _bl.Clock;

        Do.Task doTask = new(boTask.Id, boTask.IdWorker, boTask.Name, boTask.Description, boTask.MileStone,
                                    boTask.Time, createDate, boTask.WantedStartDate, boTask.StartDate, boTask.EndingDate,
                                    DeadLine, boTask.Product, boTask.Notes, boTask.Rank);
        boTask.Status = GetStatus(doTask);

        try
        {
            int idTask = _dal.Task.Create(doTask);

            if (boTask.DependenceTasks != null)
            {
                var dependencies = (from BO.TaskList dep in boTask.DependenceTasks
                                    select _dal.Dependency.Create(new Do.Dependency(0, idTask, dep.Id))).ToList();
            }
            return idTask;
        }
        catch (Do.DalAlreadyExistsException ex)
        {
            throw new BlAlreadyExistsException($"task with ID={boTask.Id} already exists", ex);
        }
    }
    /// <summary>
    /// delete the wanded task 
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BlCantBeDeleted"></exception>
    /// <exception cref="BlDoesNotExistException"></exception>
    public void Delete(int id)
    {

        if (BlApi.Factory.Get().CheckStatusProject() != BO.StatusProject.Planning)
            throw new BlCantBeDeleted("You can't delete task during execution");

        var result = from BO.TaskList task in _bl.Task.ReadAll()
                     let fullTask = _bl.Task.Read(task.Id)
                     let depTasks = fullTask.DependenceTasks
                     from dep in depTasks
                     where dep.Id == id
                     select task;         
                                                               
       if (result.Count()>0)
            throw new BlCantBeDeleted("this task can't be deleted because it has dependence tasks");


        try
        {

            _dal.Task.Delete(id);
        }
        catch (Do.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Task with ID={id} doesn't exists", ex);
        }
    }
    /// <summary>
    /// print the task with the given id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    public BO.Task? Read(int id)
    {

        Do.Task doTask = _dal.Task.Read(id)!;

        if (doTask == null)
            throw new BlDoesNotExistException($"Task with ID={id} doesnt exists");

        return new BO.Task()//create the bo task
        {
            Id = id,
            IdWorker = doTask.IdWorker,
            NameWorker = (doTask.IdWorker != null && _dal.Worker.Read(doTask.IdWorker.Value) != null) ? _dal.Worker.Read(doTask.IdWorker.Value)!.Name : null,
            Name = doTask.Name,
            Description = doTask.Description,
            MileStone = doTask.MileStone,
            Time = doTask.Time,
            CreateDate = doTask.CreateDate,
            WantedStartDate = doTask.WantedStartDate,
            StartDate = doTask?.StartDate,
            EndingDate = doTask?.EndingDate,
            DeadLine = doTask!.DeadLine,
            Product = doTask.Product,
            Notes = doTask.Notes,
            Rank = doTask.Rank,
            Status = GetStatus(doTask),
            DependenceTasks = getDependenceList(doTask)
        };

    }
    /// <summary>
    /// print all the tasks
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.TaskList> ReadAll(Func<BO.TaskList, bool>? filter = null)
    {
        IEnumerable<BO.TaskList> result;
        if (filter == null)//if there is no filter , create task list for each task
        {
            result= (from Do.Task doTask in _dal.Task.ReadAll()
                    select new BO.TaskList()
                    {
                        Id = doTask.Id,
                        Name = doTask.Name,
                        Description = doTask.Description,
                        Status = GetStatus(doTask),
                    });
        }
        else
        {//ifthere is filter create the task list but choose only the tasks that the filter match them
            result =  (from Do.Task doTask in _dal.Task.ReadAll()
                    let boTask = new BO.TaskList()
                    {
                        Id = doTask.Id,
                        Name = doTask.Name,
                        Description = doTask.Description,
                        Status = GetStatus(doTask),
                    }
                    where filter(boTask)
                    select boTask
                    );
          
        }

        var orderResult = result.OrderBy(doTask => doTask.Id); //order the worker by name
        return orderResult;
    }
    /// <summary>
    /// uptate the task to the given task
    /// </summary>
    /// <param name="boTask"></param>
    /// <exception cref="BlDoesNotExistException"></exception>
    /// <exception cref="BlWhilePlanning"></exception>
    /// <exception cref="BlDuringExecution"></exception>
    public void Update(BO.Task boTask)
    {
        var oldTask = _dal.Task.Read(boTask.Id);

        //if (BlApi.Factory.Get().CheckStatusProject() != BO.StatusProject.Planning)
        //    throw new BlCantBeUpdated("you can't update a a task since the project started");

        if (boTask.IdWorker != 0 &&_dal.Worker.Read(doWorker => doWorker.Id == boTask.IdWorker) == null)//throw match exeptions
            throw new BlDoesNotExistException($"There is no worker with ID={boTask.IdWorker}");

        //if (BlApi.Factory.Get().CheckStatusProject() == BO.StatusProject.Planning && boTask.IdWorker != 0)
        //    throw new BlWhilePlanning("you cant assign a worker while planning the project");

        if (BlApi.Factory.Get().CheckStatusProject() == BO.StatusProject.Planning && (boTask.WantedStartDate != null || boTask.StartDate!= null
            || boTask.DeadLine != null || boTask.EndingDate != null))
            throw new BlWhilePlanning("you cant update dates while planning the project");

        if (BlApi.Factory.Get().CheckStatusProject() == BO.StatusProject.Execution && boTask.Time != null)
            throw new BlDuringExecution("you cant update the task duration during the execution");

        if (BlApi.Factory.Get().CheckStatusProject() == BO.StatusProject.Execution && boTask.StartDate != null)
            throw new BlDuringExecution("you cant update the start date during the execution");
        if (boTask.Status == BO.Status.None)
            throw new BlCantBeUpdated("you cant choose None as status");

        if (boTask.StartDate != null)
            boTask.StartDate = CreateSchedule(boTask.Id, boTask.StartDate);

        if (boTask.EndingDate < _bl.Clock)
            throw new BlCantBeUpdated("this date passed");
         DateTime? DeadLine = boTask.StartDate > boTask.WantedStartDate ? boTask.StartDate + boTask.Time : boTask.WantedStartDate + boTask.Time;
           

       
            //AutometicSchedule();
        //    throw new BlCantBeUpdated("The deadLine is over");

        if(boTask.DeadLine> boTask.EndingDate && boTask.EndingDate != null)
            DeadLine = boTask.EndingDate;

        bool flag = false;

        if (boTask.EndingDate != null && boTask.EndingDate > boTask.DeadLine)
        {
           boTask.Time = boTask.Time + (boTask.EndingDate - boTask.DeadLine);
            flag = true;
            // DeadLine = boTask.StartDate > boTask.WantedStartDate ? boTask.StartDate + boTask.Time : boTask.WantedStartDate + boTask.Time;

        }

        Do.Task doTask = new(boTask.Id, boTask.IdWorker, boTask.Name, boTask.Description, boTask.MileStone,
                                    boTask.Time, boTask.CreateDate, boTask.WantedStartDate, boTask.StartDate, boTask.EndingDate,
                                    DeadLine, boTask.Product, boTask.Notes, boTask.Rank);//build the do object
        boTask.Status = GetStatus(doTask);//update the logic properties
        //boTask.DependenceTasks = getDependenceList(doTask);
        
        try
        {
            if(boTask.IdWorker!=0)
                CheckTaskForWorker(boTask);//call those functions

            if(boTask.StartDate!=null)
                CheckStartDate(boTask.Id, boTask.StartDate);

            _dal.Task.Update(doTask);
            AutometicSchedule();

            if (boTask.StartDate > boTask.DeadLine)
                throw new Exception("You start the task too late");

            if (flag)
            {                
                throw new Exception("You finished the task too late");
            }
        }
        catch (Do.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"task with ID={doTask.Id} doesnt exists", ex);
        }
    }
    /// <summary>
    /// check if the start date can bo updated
    /// </summary>
    /// <param name="idTask"></param>
    /// <param name="date"></param>
    /// <exception cref="BlDoesNotExistException"></exception>
    /// <exception cref="BlCantBeUpdated"></exception>
    private void CheckStartDate(int idTask, DateTime? date)
    {
        if (_dal.Task.Read(idTask) == null)
            throw new BlDoesNotExistException($"task with ID={idTask} doesnt exists");

        Do.Task? doTask = _dal.Task.Read(idTask);
        var result = (from BO.TaskList boTask in getDependenceList(doTask)//if the previous tasks dont have start date or the given date is sooner there the dead lines
                      let task=_dal.Task.Read(boTask.Id)
                      where task.StartDate == null || date <= task.DeadLine
                      select boTask);

        if (result.Count() > 0)
            throw new BlCantBeUpdated("this date can't be updated");

        if (getDependenceList(doTask) != null && _dal.GetStartDate()!=null && date < _dal.GetStartDate())//if the given date is sooner then the start date project
            throw new BlCantBeUpdated("this date can't be updated");
    }
    /// <summary>
    /// check if a worker can assign to a task
    /// </summary>
    /// <param name="boTask"></param>
    /// <exception cref="BlCantBeUpdated"></exception>
    private void CheckTaskForWorker(BO.Task boTask)
    {
        var oldTask = _dal.Task.Read(boTask.Id);
        if (oldTask!=null && (oldTask.IdWorker != 0 && oldTask.IdWorker != null) && oldTask.IdWorker != boTask.IdWorker)//if another worker is assign
            throw new BlCantBeUpdated("this worker can't assign this task because another worker is assigned");
        if (oldTask != null && boTask.DependenceTasks != null)
        {
            var noDoneTasks = from BO.TaskList task in boTask.DependenceTasks//if the previous tasks status is not done
                              where task.Status != BO.Status.Done
                              select boTask;
            if (noDoneTasks.Count() > 0)
                throw new BlCantBeUpdated("this worker can't assign this task because the dependences tasks didn't finished yet");
        }

        if (boTask.IdWorker is int _idworker)
        {
            Do.Worker worker = _dal.Worker.Read(_idworker)!;//if the task rank is bigger then the worker rank
            if (worker != null && boTask.Rank > (int)worker.WorkerRank)
                throw new BlCantBeUpdated("this worker can't assign this task because the rank doesn't fit");
        
        }
    }
    /// <summary>
    /// update the start dates of the tasks
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_date"></param>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    /// <exception cref="BlCantUpdateStartDateExecution"></exception>
    public DateTime? CreateSchedule(int _id, DateTime? _date)
    {
        Do.Task? _task = _dal.Task.Read(_id);

            if (_task == null)//if the task doesn't exist
                throw new BlDoesNotExistException($"this task with id ={_id} doesn't exist");
        if (getDependenceList(_task) != null)
        {
            var startDates = from BO.TaskList taskList in getDependenceList(_task)
                             where getDependenceList(_task) != null && _dal.Task.Read(taskList.Id) != null && _dal.Task.Read(taskList.Id)!.WantedStartDate == null
                             select taskList;
            if (startDates.Count() > 0)//if the previous tasks don't have start dates
                throw new BlCantUpdateStartDateExecution("You can't update the start date, because the previous tasks don't have start dates");

            var endDates = from BO.TaskList taskList in getDependenceList(_task)
                           where getDependenceList(_task) != null && _dal.Task.Read(taskList.Id) != null && _dal.Task.Read(taskList.Id)!.EndingDate > _date
                           select taskList;
           // if (endDates.Count() > 0)//if the date is sooner then the previous tasks end dates
            //    throw new BlCantUpdateStartDateExecution("You can't update the start date, because the date is sooner then the previous tasks end dates");
        }
            if (getDependenceList(_task) == null && _date < _dal.GetStartDate())//if he date is sooner then the start project date
                throw new BlCantUpdateStartDateExecution("You can't update the start date because the date is sooner then the start project date");

            if (_task.WantedStartDate != null && _date < _task.WantedStartDate)
                throw new BlCantUpdateStartDateExecution("You can't update the start date because the planned start date didn't arrive yet");
        
        Do.Task newTask = _task with { WantedStartDate = _date };
           _dal.Task.Update(newTask);
        return _date;

    }

    public IEnumerable <BO.TaskList> OptionTasks(BO.Worker worker)
    {
        var tasks = (from Do.Task task in _dal.Task.ReadAll()
                     //let task = _dal.Task.Read(taskList.Id)
                     where task.Rank <= (int)worker.WorkerRank && (GetStatus(task) == BO.Status.Unscheduled ||( GetStatus(task) == BO.Status.InJeopardy && task.IdWorker == 0) ) && getDependenceList(task).FirstOrDefault(t=>t.Status != BO.Status.Done)==null /*(getDependenceList(task).Where(t=> t.Status != BO.Status.Done)).Count()==0*/
                     select new BO.TaskList()
                     {
                         Id = task.Id,
                         Name = task.Name,
                         Description = task.Description,
                         Status = GetStatus(task),
                     });
        return tasks;
    }

    public IEnumerable<BO.TaskList> OptionSchduleTasks(BO.Worker worker)
    {
        var tasks = (from Do.Task task in _dal.Task.ReadAll()
                     //let task = _dal.Task.Read(taskList.Id)
                     where task.IdWorker==worker.Id && getDependenceList(task).FirstOrDefault(t => t.Status != BO.Status.Done) == null && task.StartDate == null && (GetStatus(task)==BO.Status.Scheduled || GetStatus(task) == BO.Status.InJeopardy)
                     select new BO.TaskList()
                     {
                         Id = task.Id,
                         Name = task.Name,
                         Description = task.Description,
                         Status = GetStatus(task),
                     });
        return tasks;
    }


    public void checkWorker(BO.Task task, int id)
    {
        BO.Worker worker = _bl.Worker.Read(id)!;
        if (task.IdWorker != id)
            throw new BlCantBeUpdated("wrong Id");
        if (task.NameWorker != worker.Name)
            throw new BlCantBeUpdated("wrong Name");
        if (task.StartDate < _bl.Clock && task.EndingDate == null)
            throw new BlCantBeUpdated("The date has passed");
        if (task.StartDate > _bl.Clock )
            throw new BlCantBeUpdated("The date hasn't arrived yet");
        if (task.EndingDate < _bl.Clock)
            throw new BlCantBeUpdated("The date has passed");
        if (task.EndingDate > _bl.Clock)
            throw new BlCantBeUpdated("The date hasn't arrived yet");
    }
}
