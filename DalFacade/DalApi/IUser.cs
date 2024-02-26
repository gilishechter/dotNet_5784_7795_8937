

namespace DalApi;

public interface IUser
{
    string Create(Do.User item);
    void Delete(int id);
    void ClearList();
    Do.User? Read(int id);
    Do.User? Read(string userName);

    void Update(Do.User item); //Updates entity object

    IEnumerable<Do.User> ReadAll(Func<Do.User, bool>? filter = null);
}
