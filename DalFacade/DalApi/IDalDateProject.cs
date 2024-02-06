

namespace DalApi;
using Do;
public interface IDalDateProject
{
    public void SetStartProjectDate(DateTime? StartDate);
    public void SetEndProjectDate(DateTime? EndDate);
    public DateTime? GetEndProjectDate();
    public DateTime? GetStartProjectDate();
}
