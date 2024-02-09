using DalApi;
using System.Diagnostics;
using System.Xml.Linq;

namespace Dal;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    public IWorker Worker => new WorkerImplementation();
    public ITask Task => new TaskImplementation();
    public IDependency Dependency => new DependencyImplementation();

    public DateTime? GetEndDate()
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        return root.ToDateTimeNullable("endDate");
    }

    public DateTime? GetStartDate()
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        return root.ToDateTimeNullable("startDate");
    }

    public void SetEndDate(DateTime? startDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        root.Element("endDate").Value = startDate.ToString()!;
        XMLTools.SaveListToXMLElement(root, "endDate");
    }

    public void SetStartDate(DateTime? startDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        root.Element("startDate").SetValue(startDate.ToString()!);
        XMLTools.SaveListToXMLElement(root, "data-config");
    }


}

