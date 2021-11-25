using System.Collections.Generic;
using UnityEngine;

public class ObjectInventory : MonoBehaviour
{
    private Dictionary<string, ICourtRecordObject> _dictionary = new Dictionary<string, ICourtRecordObject>();
    private List<ICourtRecordObject> _currentObjects = new List<ICourtRecordObject>();

    public ICourtRecordObject this[string courtRecordObjectName] => _dictionary[courtRecordObjectName];
    public int Count => _currentObjects.Count;
    public bool ContainsKey(string key) => _dictionary.ContainsKey(key);

    public void Add(string courtRecordObjectName)
    {
        _currentObjects.Remove(_dictionary[courtRecordObjectName]);
    }
    
    public void Add(ICourtRecordObject courtRecordObject)
    {
        _dictionary.Add(courtRecordObject.InstanceName, courtRecordObject);
    }

    public void Remove(string courtRecordObjectName)
    {
        _dictionary.Remove(courtRecordObjectName);
    }

    /// <summary>
    /// Returns the evidence at the specified index as an ICourtRecordObject to be used in the court record menu.
    /// </summary>
    /// <param name="index">The index of the evidence to get.</param>
    /// <returns>The evidence as an ICourtRecordObject</returns>
    public ICourtRecordObject GetObjectInList(int index)
    {
        return _currentObjects[index];
    }

    public void Substitute(string originalEvidenceName, string newEvidenceName)
    {
        _currentObjects[_currentObjects.IndexOf(_dictionary[originalEvidenceName])] = _dictionary[newEvidenceName];
    }
}
