
namespace Dal;

using DalApi;
using Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencys_xml = "dependencys";


    static Dependency GetDependency(XElement dep)
    {
        return new Dependency()
        {
            Id = dep.ToIntNullable("Id") ?? throw new FormatException("Can't convert Id"),
            DependenceTask = dep.ToIntNullable("Id") ?? throw new FormatException("Can't convert Id"),
            PrevTask = dep.ToIntNullable("Id") ?? throw new FormatException("Can't convert Id")
        };
    }

    public void ClearList(this List<Dependency> list)
    {
        list = new List<Dependency>();
        XMLTools.SaveListToXMLSerializer(list, s_dependencys_xml);
    }

    public int Create(Dependency item)
    {
        XElement? dependencys = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        int nextId = Config.NextDependenceId;
        Dependency newDependency = item with { Id = nextId };
        dependencys.Add(newDependency);
        XMLTools.SaveListToXMLElement(dependencys, s_dependencys_xml);
        return nextId;
    }

    public void Delete(int Id)
    {
        XElement? dependencys = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        XElement? toDelete = dependencys.Elements().FirstOrDefault(c => (int?)c.Element("Id") == Id);
        if(toDelete != null)
        {
            toDelete.Remove();
            XMLTools.SaveListToXMLElement(dependencys, s_dependencys_xml);
            return;
        }
        throw new DalDoesNotExistException($"this dependency with id={Id} is not exist");//throw exception
    }


    public Dependency? Read(int id)
    {

        XElement? tempDependency = XMLTools.LoadListFromXMLElement(s_dependencys_xml).Elements().FirstOrDefault(c => (int?)c.Element("Id") == id);
        return tempDependency is null ? null : GetDependency(tempDependency);
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_dependencys_xml).Elements().Select(dep => GetDependency(dep)).FirstOrDefault(filter);
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter == null)
            return XMLTools.LoadListFromXMLElement(s_dependencys_xml).Elements().Select(dep => GetDependency(dep));
        else
            return XMLTools.LoadListFromXMLElement(s_dependencys_xml).Elements().Select(dep => GetDependency(dep)).Where(filter);

    }

    public void Update(Dependency item)
    {
        XElement? dependencys = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        XElement? tempDependency = XMLTools.LoadListFromXMLElement(s_dependencys_xml).Elements().FirstOrDefault(c => (int?)c.Element("Id") == item.Id);
        if (tempDependency is null)
            throw new DalDoesNotExistException($"this dependence with id={item.Id} is not exist");//throw exception
        tempDependency.Remove();
        dependencys.Add(item);
        XMLTools.SaveListToXMLElement(dependencys, s_dependencys_xml);                    
    }
}

