
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
            if (user.id == item.id)//if the ID's equal
            {
                DataSource.Users.Remove(user);//remove the old item
                DataSource.Users.Add(item);//add the new one
                return;
            }
        }
        throw new DalDoesNotExistException($"this user with user Name={item.userName} is doesn't exist");//throw exception
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
        return item.userName!;
    }

    public void Delete(int id)
    {
        foreach (User _user in DataSource.Users)//go through the list
        {
            if (_user.id == id)//if the ID's even
            {
                DataSource.Users.Remove(_user);//remove the item from the list
                return;
            }
        }
        throw new DalDoesNotExistException($"this user with Id={id} is not exist");
    }

    public User? Read(int id)
    {
        return DataSource.Users.FirstOrDefault(obj => obj.id == id);
    }

    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        if (filter == null)//if there is no function to filtering
            return DataSource.Users.Select(item => item);//return all the list
        else
            return DataSource.Users.Where(filter);//return the list after the filtering by the function
    }

    public User? Read(string userName)
    {
        return DataSource.Users.FirstOrDefault(obj => obj.userName == userName);

    }
}
