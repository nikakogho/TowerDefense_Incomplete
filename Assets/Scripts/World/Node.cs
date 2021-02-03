using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {
    public Tower tower;

    Material mat;

    public Color defaultColor = Color.white;
    public Color hoverColor = Color.grey;
    public Color cantColor = Color.red;
    public Color canColor = Color.green;

    BuildManager manager;

    public void SetDefaultColor(Color color)
    {
        GetComponent<Renderer>().sharedMaterial.color = defaultColor = color;
    }

    void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Start()
    {
        manager = BuildManager.instance;
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if(tower == null)
        {
            mat.color = manager.CanBuild ? canColor : cantColor;
        }
        else
        {
            mat.color = hoverColor;
        }
    }

    void OnMouseExit()
    {
        mat.color = defaultColor;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (tower == null)
        {
            if (manager.CanBuild)
            {
                manager.Build(this);
                OnMouseEnter();
            }
        }
        else
        {
            manager.SelectTowerBlueprint(null);
            manager.SelectDropBlueprint(null);

            if (NodeUI.instance.gameObject.activeSelf && NodeUI.instance.node == this)
                NodeUI.instance.Deselect();
            else
                NodeUI.instance.ApplyNode(this);
        }
    }
}
