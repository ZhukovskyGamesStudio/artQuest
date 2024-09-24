using UnityEngine;

[CreateAssetMenu(fileName = "ShablonConfig", menuName = "ScriptableObjects/ShablonConfig", order = 1)]
public class ShablonConfig : RewardConfig {
    [Header("Shablon")]
    public Sprite Sprite;

    public string Name;
    public int Id;
    public int MinPixels = 10;
    public float CostPerPixel = 1;

    public override bool IsAlreadyHave() {
        return MetaCore.Instance.Inventory.Shablons[Id];
    }

    public override string GetData() {
        return Id.ToString();
    }
}