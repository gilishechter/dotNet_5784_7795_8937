using DalApi;
using Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal class UserImplementation : IUser
    {
        readonly string s_users_xml = "users";
        public void ClearList()
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
            List<User> admin = new();
            foreach (var item in users)
            {
                if (item.isAdmin)
                    admin.Add(item);
            }
            XMLTools.SaveListToXMLSerializer(admin, s_users_xml);
        }

        public string Create(User item)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
            foreach (User tempUser in users)//go through the list
            {
                if (tempUser.userName == item.userName)//if the item is already exist
                {
                    throw new DalAlreadyExistsException($"this user with user name={item.userName} is already exist");//throe exception
                }
            }
            users.Add(item);//add the object to the list
            XMLTools.SaveListToXMLSerializer(users, s_users_xml);
            return item.userName;
        }

        public void Delete(string userName)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
            foreach (User tempUser in users)
            {
                if (tempUser.userName == userName)
                {
                    users.Remove(tempUser);
                    XMLTools.SaveListToXMLSerializer(users, s_users_xml);
                    return;
                }
            }
            throw new DalDoesNotExistException($"this user with user name={userName} is not exist");//throw exception
        }

        public User? Read(string userName)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
            return users.FirstOrDefault(t => t.userName == userName);
        }

        public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
            if (filter == null)//if there is no function to filtering
                return users.Select(item => item);//return all the list
            else
                return users.Where(filter);//return the list after the filtering by the function
        }
    }
}
