namespace DalApi;
using Do;
using System.Xml.Linq;

public interface ITask: ICrud<Task>
{
    public void setStartDate(DateTime? startDate);
    public DateTime? getStartDate();
    public void setEndDate(DateTime? startDate);
    public DateTime? getEndDate();

}
