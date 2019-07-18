using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatableDictionary<Tkey,Tvalue>
{
    private List<Tkey> keyList=new List<Tkey>();
    private List<Tvalue> valueList=new List<Tvalue>();

    
    public void Add(Tkey key, Tvalue value)
    {
        keyList.Add(key);
        valueList.Add(value);
    }
    public List<Tvalue> GetAllValue(Tkey key)
    {
        int index=0;
        List<Tvalue> values = new List<Tvalue>();
        foreach (Tkey k in keyList)
        {
            if (k.Equals(key))
            {
                values.Add(valueList[index]);
            }
            index++;
        }
        return valueList;
    }
    public void Remove(int index)
    {
        keyList.Remove(keyList[index]);
        valueList.Remove(valueList[index]);
    }
}
