using UnityEngine;

[System.Serializable]
public class Blueprint
{
    public GameObject prefab;
    public Sprite icon;
    public int cost;
}

[System.Serializable]
public class BlueprintWithEffects : Blueprint
{
    public GameObject buildEffect, destroyEffect;
}