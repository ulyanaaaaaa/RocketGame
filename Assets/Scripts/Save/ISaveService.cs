using System;

public interface ISaveService
{
    public void Save(string id, object data, Action<bool> callback = null);
    public void Load<T>(string id, Action<T> callback);
    bool Exists(string s);
}