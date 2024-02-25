

namespace BlApi;

public interface IUser
{
    public void Delete(string userName);
    public string Create(BO.User user);
    public void ClearUser();
    public IEnumerable<BO.User> ReadAll(Func<BO.User, bool>? filter = null);
    public BO.User? Read(string userName);

}
