public class ShopDrop : ShopItem {
    public DropBlueprint blueprint;

    protected override Blueprint FirstBlueprint
    {
        get
        {
            return blueprint.item;
        }
    }

    public override void Select()
    {
        manager.SelectDropBlueprint(blueprint);
    }
}
