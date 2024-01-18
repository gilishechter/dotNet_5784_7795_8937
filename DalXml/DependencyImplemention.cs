
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

        XElement? dependencys = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        XElement? toRead = dependencys.Elements().FirstOrDefault(c => (int?)c.Element("Id") == id);
        if(toRead != null)
        {
            int _DependenceTask = int.TryParse((string?)dependencys.Element("DependenceTask"), out var result) ? (int)result : throw new FormatException("can't convert");
            int _PrevTask = int.TryParse((string?)dependencys.Element("PrevTask"), out var result1) ? (int)result1 : throw new FormatException("can't convert");

            return new Dependency(id, _DependenceTask, _PrevTask);
       }
        return null;
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement? dependencys = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        XElement? toRead = dependencys.Elements().FirstOrDefault(filter);
        if (toRead != null)
        {
            int _Id = int.TryParse((string?)dependencys.Element("Id"), out var result1) ? (int)result1 : throw new FormatException("can't convert");
            int _DependenceTask = int.TryParse((string?)dependencys.Element("DependenceTask"), out var result) ? (int)result : throw new FormatException("can't convert");
            int _PrevTask = int.TryParse((string?)dependencys.Element("PrevTask"), out var result2) ? (int)result2 : throw new FormatException("can't convert");

            return new Dependency(_Id, _DependenceTask, _PrevTask);
        }
        return null;
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml);
        if (filter == null)//if there is no function to filtering
            return dependencys.Select(item => item);//return all the list
        else
            return dependencys.Where(filter);//return the list after the filtering by the function
    }

    public void Update(Dependency item)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml);
        foreach (Dependency tempDependency in dependencys)
        {
            if (tempDependency.Id == item.Id)
            {
                dependencys.Remove(tempDependency);
                dependencys.Add(item);
                XMLTools.SaveListToXMLSerializer(dependencys, s_dependencys_xml);
                return;
            }
        }
        throw new DalDoesNotExistException($"this dependency with id={item.Id} is not exist");//throw exception
    }
}

