using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableRow
{
    Dictionary<string, object> dict;
    public int Length;
    public TableRow()
    {
        dict = new Dictionary<string, object>();
        Length = 0;

    }

    public void SetInt(string key, int value)
    {
        dict.Add(key, value);
        Length = dict.Count;
    }

    public void SetFloat(string key, float value)
    {
        dict.Add(key, value);
        Length = dict.Count;

    }

    public void SetString(string key, string name)
    {
        dict.Add(key, name);
        Length = dict.Count;

    }

    public object GetObject(string key)
    {
        return dict[key];

    }

    public string[] GetKeys()
    {
        var keyList = new List<string>();

        foreach (string key in dict.Keys)
            keyList.Add(key);

        string[] keyArray = new string[keyList.Count];
        for (int i = 0; i < keyArray.Length; i++)
            keyArray[i] = keyList[i];
        return keyArray;
    }
}
