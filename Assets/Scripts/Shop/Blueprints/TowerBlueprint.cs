using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Shop/Tower")]
public class TowerBlueprint : ScriptableObject
{
    new public string name;

    [TextArea(3, 20)]
    public string description;

    public BlueprintWithEffects firstLevel;
    public BlueprintWithEffects[] upgrades;

    public BlueprintWithEffects LevelBlueprint(int level)
    {
        return level == 1 ? firstLevel : upgrades[level - 2];
    }
}