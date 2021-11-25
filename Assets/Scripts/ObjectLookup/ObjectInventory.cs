using System.Collections.Generic;
using UnityEngine;

public class ObjectInventory : MonoBehaviour
{
    private Dictionary<string, ICourtRecordObject> _dictionary = new Dictionary<string, ICourtRecordObject>();

    public ICourtRecordObject this[string courtRecordObjectName] => _dictionary[courtRecordObjectName];

    public void Add(ICourtRecordObject courtRecordObject)
    {
        _dictionary.Add(courtRecordObject.InstanceName, courtRecordObject);
    }

    public void Remove(string courtRecordObjectName)
    {
        _dictionary.Remove(courtRecordObjectName);
    }
    
    public bool ContainsKey(string key) => _dictionary.ContainsKey(key);
}
