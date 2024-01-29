

namespace BO;

public class TaskList
{
   public WorkerTask? TaskDetails { get; set; }
    public string? Description { get; set; }
   
    public BO.Status Status { get; set; }
    public override string ToString() => this.ToStringProperty();
}
