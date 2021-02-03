using UnityEngine;

public class BuildManager : MonoBehaviour {
    TowerBlueprint selectedTowerBlueprint;
    DropBlueprint selectedDropBlueprint;

    public bool CanBuild { get { return selectedTowerBlueprint != null && PlayerStats.Money >= selectedTowerBlueprint.firstLevel.cost; } }
    public bool CanDrop { get { return selectedDropBlueprint != null && PlayerStats.Money >= selectedDropBlueprint.item.cost; } }

    public LayerMask pathMask;

    public static BuildManager instance;

    public GameObject defaultBuildEffect, defaultDestroyEffect;

    void Awake()
    {
        instance = this;
    }

    public void Build(Node node)
    {
        Vector3 spawnPos = node.transform.position;
        spawnPos.y = 0;

        node.tower = Instantiate(selectedTowerBlueprint.firstLevel.prefab, spawnPos, Quaternion.identity).GetComponent<Tower>();
        node.tower.transform.parent = node.transform;

        var first = selectedTowerBlueprint.firstLevel;
        var effect = first.buildEffect;

        var parent = node.transform;

        var realEffect = effect != null ? effect : defaultBuildEffect;
        
        Destroy(Instantiate(realEffect, spawnPos, Quaternion.identity, parent), 3.4f);

        PlayerStats.Money -= selectedTowerBlueprint.firstLevel.cost;
    }

    public void Drop(Vector3 point)
    {
        Instantiate(selectedDropBlueprint.item.prefab, point + Vector3.up * selectedDropBlueprint.offsetY, Quaternion.identity, transform);
        PlayerStats.Money -= selectedDropBlueprint.item.cost;
    }

    public void SelectTowerBlueprint(TowerBlueprint blueprint)
    {
        selectedDropBlueprint = null;
        selectedTowerBlueprint = blueprint;
    }

    public void SelectDropBlueprint(DropBlueprint blueprint)
    {
        selectedDropBlueprint = blueprint;
        selectedTowerBlueprint = null;
    }
}
