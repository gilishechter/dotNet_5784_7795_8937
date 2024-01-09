namespace DalApi;
using Do;
public interface IDependence
{
    int Create(Dependencies item); //Creates new entity object in DAL
    Dependencies? Read(int id); //Reads entity object by its ID 
    List<Dependencies> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(Dependencies item); //Updates entity object
    void Delete(int Id); //Deletes an object by its Id

}
