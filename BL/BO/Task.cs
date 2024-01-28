namespace BO;
/// <summary>
/// logic properties task
/// </summary>
public class Task
{
    public int Id {get;init; }
    public int IdWorker { get;set;}
    public string? Name { get;init;} 
    public string? Description { get;set;}
    public bool MileStone { get;set;}
    public TimeSpan? Time { get;set;}
    public DateTime? CreateDate { get; init; }
    public DateTime? WantedStartDate { get; set;}
    public DateTime? StartDate { get; set; }
    public DateTime? EndingDate { get; set; }
    public DateTime? DeadLine { get; set; }
    public string? Product { get; init; }
    public string? Notes { get; set; }
    public int Rank { get; init;}
    public Status Status { get; set; }
    public IEnumerable<BO.TaskList>? DependenceTasks { get; set;}
    public override string ToString() => this.ToStringProperty();

}
