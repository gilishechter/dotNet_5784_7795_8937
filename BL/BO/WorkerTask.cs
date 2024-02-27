

namespace BO;

public class WorkerTask
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public override string ToString() => this.ToStringProperty();
}
