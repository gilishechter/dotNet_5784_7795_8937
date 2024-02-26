
using DalApi;
using Do;
using System.Linq;
using System.Collections.Generic;
namespace Dal;

internal class UserImplementation : IUser
{
    public void ClearList()
    {
        foreach (var item in DataSource.Users)
        {
            if (!item.isAdmin)
            {
                DataSource.Users.Remove(item);
            }
        }
    }

    public string Create(User item)
    {
        foreach (User _user in DataSource.Users)//go through the list
        {
            if (_user.Id == item.Id)//if the item is already exist
            {
                throw new DalAlreadyExistsException($"this user with id={item.Id} is already exist");//throe exception
            }
        }

        DataSource.Users.Add(item);//add the object to the list
        return item.userName;
    }

    public void Delete(string userName)
    {
        foreach (User _user in DataSource.Users)//go through the list
        {
            if (_user.userName == userName)//if the ID's even
            {
                DataSource.Users.Remove(_user);//remove the item from the list
                return;
            }
        }
        throw new DalDoesNotExistException($"this user with user name={userName} is not exist");
    }

    public User? Read(string userName)
    {
        return DataSource.Users.FirstOrDefault(obj => obj.userName == userName);
    }

    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        if (filter == null)//if there is no function to filtering
            return DataSource.Users.Select(item => item);//return all the list
        else
            return DataSource.Users.Where(filter);//return the list after the filtering by the function
    }
}
