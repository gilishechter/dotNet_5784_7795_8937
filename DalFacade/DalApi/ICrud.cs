﻿using Do;

namespace DalApi;
public interface ICrud<T> where T : class
{
    int Create(T item); //Creates new entity object in DAL
    T? Read(int id); //Reads entity object by its ID 
    IEnumerable<T?> ReadAll(Func<T, bool>? filter = null); // stage 2

    void Update(T item); //Updates entity object
    void Delete(int Id); //Deletes an object by its Id

    T? Read(Func<T, bool> filter); // stage 2

    void ClearList();


}
