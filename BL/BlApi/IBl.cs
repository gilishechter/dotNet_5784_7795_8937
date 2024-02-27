﻿using BO;
using DalApi;
using System.Reflection.Metadata.Ecma335;
namespace BlApi;

public interface IBl
{

    public ITask Task { get;}
    public ITaskList TaskList { get;}
    public IWorker Worker { get;}
    public IWorkerTask WorkerTask { get;}
    public IUser User { get;}

    public void Clear();
    public void InitializeDB() => DalTest.Initialization.Do();


    public void SetStartProject(DateTime? date);
    public void SetEndProject(DateTime? date);
    public DateTime? GetStartProject();
    public DateTime? GetEndProject();
    public StatusProject CheckStatusProject();
    //public void AutometicSchedule(); we didn't do the bonus yet

    #region
    public DateTime Clock { get; }
    public void SetClockHour();
    public void SetClockYear();
    public void SetClockDay();

    public void ResetClock();


    #endregion
}
