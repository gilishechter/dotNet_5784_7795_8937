

namespace DalApi;

public interface IUser
{
    string Create(Do.User item);
    void Delete(string userName);
    void ClearList();
    Do.User? Read(string userName);

    void Update(Do.User item); //Updates entity object

    IEnumerable<Do.User> ReadAll(Func<Do.User, bool>? filter = null);
}
