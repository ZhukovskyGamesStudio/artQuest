using UnityEngine;

[CreateAssetMenu(fileName = "PixelConfig", menuName = "ScriptableObjects/PixelConfig", order = 2)]
public class PixelConfig : RewardConfig {
    [Header("Pixel")]
    public Color32 Color = new Color32(0, 0, 0, 255);

    public string Name;
    public FighterStats Stats = new FighterStats();

    public override string GetData() {
        return Name;
    }
}