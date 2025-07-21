using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    //随机获取List<T>里的一个，然后在列表中删除它
    public static T Draw<T> (this List<T> list)
    {
        if (list.Count == 0) return default;
        int r = Random.Range(0, list.Count);
        T t = list[r];
        list.Remove(t);
        return t;
    }
}

