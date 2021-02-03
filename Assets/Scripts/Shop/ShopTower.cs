public class ShopTower : ShopItem {
    public TowerBlueprint blueprint;

    protected override Blueprint FirstBlueprint { get
        {
            return blueprint.firstLevel;
        }
    }

    public override void Select()
    {
        manager.SelectTowerBlueprint(blueprint);
    }
}
