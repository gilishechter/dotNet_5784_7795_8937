

namespace BlApi;

public interface IUser
{
    public void Delete(int id);
    public string Create(BO.User user);
    public void ClearUser();
    public IEnumerable<BO.User> ReadAll(Func<BO.User, bool>? filter = null);
    public BO.User? Read(int id);
    public BO.User? Read(string userName);
    public void Update(BO.User user);


}
