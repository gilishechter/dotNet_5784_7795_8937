using BO;

namespace BlApi;

internal interface IBlDateProject
{
    public void createStartProjectDate();
    public void createEndProjectDate();
    public DateTime? EndProjectDate();
    public DateTime? startProjectDate();
}
