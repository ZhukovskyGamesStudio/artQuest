using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardsConfig", menuName = "ScriptableObjects/RewardsConfig", order = 3)]
public class RewardsConfig : ScriptableObject {
    public List<RewardConfig> Rewards = new List<RewardConfig>();
}