using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour {
    public static NodeUI instance;
    public Node node;

    public Sprite maxIcon;

    public Button upgradeButton, sellButton;
    public Button[] targetModeButtons;
    public Image upgradeImage;
    public Text upgradeText, infoText, sellText;

    int upgradeCost, sellValue;

    public Vector3 offset = new Vector3(0, 10, 0);

    void Awake()
    {
        instance = this;

        gameObject.SetActive(false);
    }

    public void ApplyNode(Node node)
    {
        gameObject.SetActive(true);
        this.node = node;

        foreach (Button button in targetModeButtons) button.interactable = true;
        targetModeButtons[(int)node.tower.targetMode].interactable = false;

        transform.position = node.transform.position + offset;

        bool isMaxLevel = node.tower.level == node.tower.blueprint.upgrades.Length + 1;

        if(!isMaxLevel)
        upgradeCost = node.tower.blueprint.upgrades[node.tower.level - 1].cost;

        upgradeButton.interactable = !isMaxLevel && upgradeCost <= PlayerStats.Money;
        upgradeImage.sprite = isMaxLevel ? maxIcon :  node.tower.blueprint.upgrades[node.tower.level - 1].icon;
        upgradeText.text = isMaxLevel ? "" : ("UPPGRADE\n$" + upgradeCost);

        sellValue = (node.tower.level == 1 ? node.tower.blueprint.firstLevel.cost : node.tower.blueprint.upgrades[node.tower.level - 2].cost) / 2;

        sellText.text = "SELL\n$" + sellValue;

        infoText.text = node.tower.blueprint.description;
    }

    public void Upgrade()
    {
        Vector3 spawnPoint = node.transform.position;
        spawnPoint.y = 0;
        var item = node.tower.blueprint.upgrades[node.tower.level - 1];

        GameObject newTower = Instantiate(item.prefab, spawnPoint, Quaternion.identity);
        newTower.transform.parent = node.transform;

        DestroyOld();

        var effect = item.buildEffect;

        Destroy(Instantiate(effect != null ? effect : BuildManager.instance.defaultBuildEffect, spawnPoint, Quaternion.identity), 5);

        PlayerStats.Money -= upgradeCost;

        node.tower = newTower.GetComponent<Tower>();

        Deselect();
    }

    void DestroyOld()
    {
        var effect = node.tower.blueprint.LevelBlueprint(node.tower.level).destroyEffect;
        var realEffect = effect != null ? effect : BuildManager.instance.defaultDestroyEffect;

        Destroy(Instantiate(realEffect, node.tower.transform.position, Quaternion.identity), 5);
        Destroy(node.tower.gameObject);
    }

    public void Sell()
    {
        DestroyOld();
        PlayerStats.Money += sellValue;

        Deselect();
    }

    public void Deselect()
    {
        gameObject.SetActive(false);
    }

    public void SetTargetMode(int mode)
    {
        targetModeButtons[(int)node.tower.targetMode].interactable = true;
        node.tower.targetMode = (Tower.TargetMode)mode;
        targetModeButtons[mode].interactable = false;
    }
}
