

namespace BO;

public class TaskList
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Name { get; init; }
    public BO.Status Status { get; set; }
}
