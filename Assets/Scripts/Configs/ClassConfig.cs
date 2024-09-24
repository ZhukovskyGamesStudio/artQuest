using UnityEngine;

[CreateAssetMenu(fileName = "ClassConfig", menuName = "ScriptableObjects/ClassConfig", order = 5)]
public class ClassConfig : ScriptableObject {
    public FighterClassType Type;
    public int Cost;
    public FighterStats DefaultStats;
    public Fighter Fighter;
    public Sprite Icon64;
}