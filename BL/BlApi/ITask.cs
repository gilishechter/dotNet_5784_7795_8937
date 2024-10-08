﻿namespace BlApi;

public interface ITask
{
    public IEnumerable<BO.TaskList> ReadAll(Func<BO.TaskList, bool>? filter = null);
    public void Update(BO.Task task);
    public void Delete(int Id);
    public int Create(BO.Task task);
    public BO.Task? Read(int Id);
    public IEnumerable<BO.TaskList> OptionTasks(BO.Worker worker);
    public IEnumerable<BO.TaskList> OptionSchduleTasks(BO.Worker worker);
    public void AutometicSchedule();
    public IEnumerable<BO.GanttDetails> GetDetailsToGantt(Func<BO.GanttDetails, bool>? filter = null);
    public void checkWorker(BO.Task task, int id);



}
