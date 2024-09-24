using System.Collections;

public class MetaCore : InitableManager {
    public static MetaCore Instance;
    public Inventory Inventory { get; private set; }
    public Tavern Tavern { get; private set; }
    public Party Party { get; private set; }

    public override IEnumerator Init() {
        Instance = this;
        Inventory = new Inventory();
        Tavern = new Tavern();
        Party = new Party();
        InitEmpty();
        return base.Init();
    }

    private void InitEmpty() {
        Inventory.InitEmpty();
        Tavern.InitEmpty();
    }
}