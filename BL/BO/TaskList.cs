

namespace BO;

public class TaskList
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; set; }
   
    public BO.Status Status { get; set; }
    public override string ToString() => this.ToStringProperty();
}
