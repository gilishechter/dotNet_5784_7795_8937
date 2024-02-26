
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

    public void Update(User item)
    {
        foreach (User user in DataSource.Users)//go through the list
        {
            if (user.userName == item.userName)//if the ID's equal
            {
                DataSource.Users.Remove(user);//remove the old item
                DataSource.Users.Add(item);//add the new one
                return;
            }
        }
        throw new DalDoesNotExistException($"this worker with id={item.Id} is not exist");//throw exception
    }
    public string Create(User item)
    {
        foreach (User _user in DataSource.Users)//go through the list
        {
            if (_user.userName == item.userName)//if the item is already exist
            {
                throw new DalAlreadyExistsException($"this user with user name={item.userName} is already exist");//throe exception
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
