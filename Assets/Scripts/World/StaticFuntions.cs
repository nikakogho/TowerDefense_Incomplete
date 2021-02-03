using UnityEngine;
using System.Collections.Generic;

public static class StaticFuntions
{
    public static Vector3 Clamp(this Vector3 current, Vector3 min, Vector3 max)
    {
        return new Vector3(Mathf.Clamp(current.x, min.x, max.x), Mathf.Clamp(current.y, min.y, max.y), Mathf.Clamp(current.z, min.z, max.z));
    }

    public static Vector2 ToXZ(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }

    public static Value TryGettingValue<Key, Value>(this Dictionary<Key, Value> dictionary, Key key) where Value : class
    {
        try
        {
            return dictionary[key];
        }
        catch
        {
            return null;
        }
    }

    public static float ToFloat(this object o)
    {
        return System.Convert.ToSingle(o);
    }

    public static List<T> ToList<T>(this T[] array)
    {
        List<T> list = new List<T>();

        foreach (T item in array) list.Add(item);

        return list;
    }
}
