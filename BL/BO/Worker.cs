namespace BO;
/// <summary>
/// logic properties worker
/// </summary>
public class Worker
{
    public int Id { get;init ; }
    public Rank WorkerRank { get;set ;}
    public double HourPrice { get;set ;}
    public string? Name { get;init;}
    public string? Email { get;set ;}
    public WorkerTask? WorkerTask { get;set ;}
    public override string ToString() => this.ToStringProperty();

}
