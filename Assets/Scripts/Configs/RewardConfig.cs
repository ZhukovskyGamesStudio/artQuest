using UnityEngine;

[CreateAssetMenu(fileName = "RewardConfig", menuName = "ScriptableObjects/RewardConfig", order = 2)]
public class RewardConfig : ScriptableObject {
    [Header("Reward")]
    public int MinWave = 0;

    public int MaxWave = 1000;
    public float Weight = 0;
    public RewardType Type = RewardType.None;
    public int CostWavePoints = 5;
    public bool IsExclusive;

    public virtual bool IsAlreadyHave() {
        return false;
    }

    public virtual string GetData() {
        return "";
    }
}