using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTable<T>
{
    public readonly List<T> dataList;
    public readonly Dictionary<long, T> dataMap;    
}