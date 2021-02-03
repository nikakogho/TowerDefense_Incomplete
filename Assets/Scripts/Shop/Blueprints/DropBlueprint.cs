using UnityEngine;

[CreateAssetMenu(fileName = "New Drop", menuName = "Shop/Drop")]
public class DropBlueprint : ScriptableObject
{
    new public string name;
    public float offsetY;

    public Blueprint item;
}
