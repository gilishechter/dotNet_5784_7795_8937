using BO;

namespace BlApi;

internal interface IBlDateProject
{
    public void SetStartProjectDate(DateTime? StartDate);
    public void SetEndProjectDate(DateTime? EndDate);
    public DateTime? GetEndProjectDate();
    public DateTime? GetStartProjectDate();
}
