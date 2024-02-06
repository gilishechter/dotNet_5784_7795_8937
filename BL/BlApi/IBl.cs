﻿

using BO;

namespace BlApi;

public interface IBl
{
    public ITask Task { get;}
    public ITaskList TaskList { get;}
    public IWorker Worker { get;}
    public IWorkerTask WorkerTask { get;}

    public static DateTime? StartDateProject { get;} = null;
    public static DateTime? EndDateProject { get; } = null;

    public StatusProject CheckStatusProject();
    public void AutometicSchedule();
}
