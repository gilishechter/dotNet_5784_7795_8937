namespace BlApi;

public interface IWorker
{
    public IEnumerable<BO.Worker> ReadAll(Func<BO.Worker, bool>? filter = null);
    public void Update(BO.Worker worker);
    public void Delete(int Id);
    public int Create(BO.Worker worker);
    public BO.Worker? Read(int Id);
    public IEnumerable<BO.Worker> RankGroup(int rank);
    public void ClearWorker();

}
