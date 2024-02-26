

using BlApi;
using BO;
using Do;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace BlImplementation;

internal class UserImplementation : IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void ClearUser()
    {
        _dal.User.ClearList();
    }

    public string Create(BO.User user)
    {
        Do.User newUser = new(user.userName, user.password, user.isAdmin,user.Id);
        return _dal.User.Create(newUser);
    }

    public void Delete(int id)
    {
        _dal.User.Delete(id);
    }

    public BO.User? Read(int id)
    {
        Do.User newUser = _dal.User.Read(id)!;
        if(newUser == null)
        {
            throw new BlDoesNotExistException($"this user with id={id} doesn't exist");
        }
        return new BO.User()
        {
            userName = newUser.userName,
            password = newUser.password,
            isAdmin = newUser.isAdmin,
            Id = newUser.id
        };
    }

    public BO.User? Read(string userName)
    {
        Do.User newUser = _dal.User.Read(userName)!;
        if (newUser == null)
        {
            throw new BlDoesNotExistException($"this user with user Name={userName} doesn't exist");
        }
        return new BO.User()
        {
            userName = newUser.userName,
            password = newUser.password,
            isAdmin = newUser.isAdmin,
            Id = newUser.id
        };
    }

    public IEnumerable<BO.User> ReadAll(Func<BO.User, bool>? filter = null)
    {
        IEnumerable<BO.User> result;
        if (filter == null)//if there is no filter , create task list for each task
        {
            result = (from Do.User doUser in _dal.User.ReadAll()
                      select new BO.User()
                      {
                          Id = doUser.id,
                          password = doUser.password,
                          isAdmin=doUser.isAdmin,
                          userName = doUser.userName
                      });
        }
        else
        {//ifthere is filter create the task list but choose only the tasks that the filter match them
            result = (from Do.User doUser in _dal.User.ReadAll()
                      let boUser = new BO.User()
                      {
                          password = doUser.password,
                          Id = doUser.id,
                          isAdmin = doUser.isAdmin,
                          userName = doUser.userName,
                      }
                      where filter(boUser)
                      select boUser
                    );

        }
        return result;
    }

    public void Update(BO.User user)
    {
        Do.User doUser = new( user.userName, user.password, user.isAdmin,user.Id );
        _dal.User.Update(doUser);

    }
}
