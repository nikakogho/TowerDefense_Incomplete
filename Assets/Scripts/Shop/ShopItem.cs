using UnityEngine;
using UnityEngine.UI;

public abstract class ShopItem : MonoBehaviour {
    Text priceText;
    Image image;
    protected BuildManager manager;

    protected abstract Blueprint FirstBlueprint { get; }

    [ContextMenu("Apply")]
    void Apply()
    {
        priceText = GetComponentInChildren<Text>();
        priceText.text = "$" + FirstBlueprint.cost;

        image = GetComponent<Image>();
        image.sprite = FirstBlueprint.icon;
    }

    void Start()
    {
        Apply();
        manager = BuildManager.instance;
    }

    public abstract void Select();
}
