using UnityEngine;

[CreateAssetMenu(fileName = "DecorationShablonConfig", menuName = "ScriptableObjects/DecorationShablonConfig", order = 1)]
public class DecorationShablonConfig : ShablonConfig {
    public DecorationType DecorationType;

    [TextArea]
    public string CustomStatsText;

    public float PowerPerPixel;
}