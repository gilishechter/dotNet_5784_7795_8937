namespace BO;

public class GanttDetails
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public string? Dependencies { get; set; }
    public int StartOffset { get; set; }
    public int TasksDays { get; set; }
    public int EndOffset { get; set; }
    public BO.Status Status { get; set; }
    public override string ToString() => this.ToStringProperty();

}