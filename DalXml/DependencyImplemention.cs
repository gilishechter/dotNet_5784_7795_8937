
namespace Dal;

using DalApi;
using Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    readonly string _dependencys_xml = "dependencys";
    //static readonly string s_data_config_xml = "data-config";

    private const string _entity_name = nameof(Dependency);
    private const string _id = nameof(Dependency.Id);
    private const string _dependenceTask = nameof(Dependency.DependenceTask);
    private const string _prevTask = nameof(Dependency.PrevTask);


    static Dependency getDependencyFromXElement(XElement dep)
        => new Dependency()
        {
            Id = dep.ToIntNullable(_id) ?? throw new FormatException("Can't convert Id"),
            DependenceTask = dep.ToIntNullable(_dependenceTask) ?? throw new FormatException("Can't convert dependence task"),
            PrevTask = dep.ToIntNullable(_prevTask) ?? throw new FormatException("Can't convert pravious task")
        };

    static XElement getXElementFromDependency(Dependency dependency)
    =>
         new XElement(_entity_name,
            new XElement(_id, dependency.Id),
            new XElement(_dependenceTask, dependency.DependenceTask),
            new XElement(_prevTask, dependency.PrevTask));

    public void ClearList()
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(_dependencys_xml);
        dependencies.Clear();
        XMLTools.SaveListToXMLSerializer(dependencies, _dependencys_xml);
        //XMLTools.SaveListToXMLElement(new XElement("config", new XElement("NextTaskId", 0), new XElement("NextDependencyId", 0)), s_data_config_xml);
    }

    public int Create(Dependency item)
    {
        XElement? dependencys = XMLTools.LoadListFromXMLElement(_dependencys_xml);
        int nextId = Config.NextDependenceId;
        Dependency newDependency = item with { Id = nextId };
        dependencys.Add(getXElementFromDependency(newDependency));
        XMLTools.SaveListToXMLElement(dependencys, _dependencys_xml);
        return nextId;
    }

    public void Delete(int Id)
    {
        XElement? dependencys = XMLTools.LoadListFromXMLElement(_dependencys_xml);
        XElement? toDelete = dependencys.Elements().FirstOrDefault(c => (int?)c.Element("Id") == Id);
        if (toDelete != null)
        {
            toDelete.Remove();
            XMLTools.SaveListToXMLElement(dependencys, _dependencys_xml);
            return;
        }
        throw new DalDoesNotExistException($"this dependency with id={Id} is not exist");//throw exception
    }


    public Dependency? Read(int id)
    {

        XElement? tempDependency = XMLTools.LoadListFromXMLElement(_dependencys_xml).Elements().FirstOrDefault(c => (int?)c.Element("Id") == id);
        return tempDependency is null ? null : getDependencyFromXElement(tempDependency);
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(_dependencys_xml).Elements().Select(dep => getDependencyFromXElement(dep)).FirstOrDefault(filter);
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter == null)
            return XMLTools.LoadListFromXMLElement(_dependencys_xml).Elements().Select(dep => getDependencyFromXElement(dep));
        else
            return XMLTools.LoadListFromXMLElement(_dependencys_xml).Elements().Select(dep => getDependencyFromXElement(dep)).Where(filter);

    }

    public void Update(Dependency item)
    {
        XElement? dependencys = XMLTools.LoadListFromXMLElement(_dependencys_xml);
        XElement? tempDependency = dependencys.Elements().FirstOrDefault(c => (int?)c.Element("Id") == item.Id);
        if (tempDependency is null)
            throw new DalDoesNotExistException($"this dependence with id={item.Id} is not exist");//throw exception
    
        tempDependency.Remove();
        dependencys.Add(getXElementFromDependency(item));
        XMLTools.SaveListToXMLElement(dependencys, _dependencys_xml);
    }
}

