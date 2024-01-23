
namespace Dal;

using DalApi;
using Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
/// <summary>
/// XMLElement method
/// </summary>
internal class DependencyImplementation : IDependency
{
    readonly string _dependencys_xml = "dependencys";

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
    /// <summary>
    /// clear the xml file
    /// </summary>
    public void ClearList()
    {
        XElement dependencies = XMLTools.LoadListFromXMLElement(_dependencys_xml);
        dependencies.RemoveAll();
        Config.NextDependenceId = 1;
        XMLTools.SaveListToXMLElement(dependencies, _dependencys_xml);
       
    }
    /// <summary>
    /// Creates new entity object in XML
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependency item)
    {
        XElement? dependencys = XMLTools.LoadListFromXMLElement(_dependencys_xml);//load the list as XML element
        int nextId = Config.NextDependenceId;//run id
        Dependency newDependency = item with { Id = nextId };//build new dependency
        dependencys.Add(getXElementFromDependency(newDependency));//add to the list
        XMLTools.SaveListToXMLElement(dependencys, _dependencys_xml);//save in the file
        return nextId;
    }
    /// <summary>
    /// Deletes an object by its Id
    /// </summary>
    /// <param name="Id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int Id)
    {
        XElement? dependencys = XMLTools.LoadListFromXMLElement(_dependencys_xml);//load the list as XML element
        XElement? toDelete = dependencys.Elements().FirstOrDefault(c => (int?)c.Element("Id") == Id);//return the element that it's id is equl to the wanted one
        if (toDelete != null)
        {
            toDelete.Remove();//delete the element
            XMLTools.SaveListToXMLElement(dependencys, _dependencys_xml);//save the updated list in the file
            return;
        }
        throw new DalDoesNotExistException($"this dependency with id={Id} is not exist");//throw exception
    }

    /// <summary>
    ///  Reads entity object by its ID 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {

        XElement? tempDependency = XMLTools.LoadListFromXMLElement(_dependencys_xml).Elements().FirstOrDefault(c => (int?)c.Element("Id") == id);
        //load from the list the wanted element by it's id
        return tempDependency is null ? null : getDependencyFromXElement(tempDependency);//if it's exist return the element, else return null
    }
    /// <summary>
    ///  Reads entity object by the function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(_dependencys_xml).Elements().Select(dep => getDependencyFromXElement(dep)).FirstOrDefault(filter);
        //load from the list the wanted element by the function
    }
    /// <summary>
    ///  return all the list or according to the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter == null)
            return XMLTools.LoadListFromXMLElement(_dependencys_xml).Elements().Select(dep => getDependencyFromXElement(dep));////return all the list
        else
            return XMLTools.LoadListFromXMLElement(_dependencys_xml).Elements().Select(dep => getDependencyFromXElement(dep)).Where(filter);////return the list after the filtering by the function

    }
    /// <summary>
    /// updates entity object
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Dependency item)
    {
        XElement? dependencys = XMLTools.LoadListFromXMLElement(_dependencys_xml);//load the list as XMl element from the file
        XElement? tempDependency = dependencys.Elements().FirstOrDefault(c => (int?)c.Element("Id") == item.Id);//return the element that it's id is equl to the wanted one
        if (tempDependency is null)
            throw new DalDoesNotExistException($"this dependence with id={item.Id} is not exist");//throw exception
    
        tempDependency.Remove();//if it exist, remove it
        dependencys.Add(getXElementFromDependency(item));//add the updated dependence 
        XMLTools.SaveListToXMLElement(dependencys, _dependencys_xml);//save te new list as XML element
    }
}

