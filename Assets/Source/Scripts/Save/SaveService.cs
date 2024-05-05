using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SaveService : ISaveService
{
    public void Save(string id, object data, Action<bool> callback = null)
    {
        string path = BuildPath(id);
        string json = JsonConvert.SerializeObject(data);

        using (var fileStream = new StreamWriter(path))
        {
            fileStream.Write(json);
        }
        
        callback?.Invoke(true);
    }

    public void Load<T>(string id, Action<T> callback)
    {
        string path = BuildPath(id);

        using (var fileStream = new StreamReader(path))
        {
            var json = fileStream.ReadToEnd();
            var data = JsonConvert.DeserializeObject<T>(json);  
            
            callback.Invoke(data);
        }
    }
    
    public bool Exists(string id)
    {
        string path = BuildPath(id);
        return File.Exists(path); 
    }

    private string BuildPath(string id)
    {
        return Path.Combine(Application.persistentDataPath, id);
    }
}