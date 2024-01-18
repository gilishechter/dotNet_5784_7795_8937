
namespace Dal;

using DalApi;
using Do;
using System;
using System.Collections.Generic;
using System.Linq;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencys_xml = "dependencys";

    public int Create(Dependency item)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml);
        int nextId = Config.NextDependenceId;
        Dependency newDependency = item with { Id = nextId };
        dependencys.Add(newDependency);
        XMLTools.SaveListToXMLSerializer(dependencys, s_dependencys_xml);
        return nextId;
    }

    public void Delete(int Id)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml);
        foreach (Dependency tempDependency in dependencys)
        {
            if (tempDependency.Id == Id)
            {
                dependencys.Remove(tempDependency);
                XMLTools.SaveListToXMLSerializer(dependencys, s_dependencys_xml);
                return;
            }
        }
        throw new DalDoesNotExistException($"this dependency with id={Id} is not exist");//throw exception
    }


    public Dependency? Read(int id)
    {
    List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml);
    return dependencys.FirstOrDefault(t => t.Id == id);
}

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml);
        return dependencys.FirstOrDefault(filter);
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

