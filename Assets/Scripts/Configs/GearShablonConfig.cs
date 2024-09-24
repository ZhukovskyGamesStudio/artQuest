using UnityEngine;

[CreateAssetMenu(fileName = "GearShablonConfig", menuName = "ScriptableObjects/GearShablonConfig", order = 1)]
public class GearShablonConfig : ShablonConfig {
    public GearType GearType;
    public FighterStats Stats = new FighterStats();
}