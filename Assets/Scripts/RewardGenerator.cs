using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardGenerator {
    private readonly RewardsConfig _rewardsConfig;

    public RewardGenerator(RewardsConfig rewardsConfig) {
        _rewardsConfig = rewardsConfig;
    }

    public List<Reward> GenerateRewardForWave(int wave, int wavePoints) {
        List<Reward> rewards = new List<Reward>();

        int wavePointsLeft = wavePoints;
        List<RewardConfig> availableRewards = _rewardsConfig.Rewards.Where(r =>
            r.MinWave <= wave && r.MaxWave >= wave && r.CostWavePoints <= wavePointsLeft && (!r.IsExclusive || !r.IsAlreadyHave())).ToList();
        while (wavePointsLeft > 0) {
            List<RewardConfig> tmpRewards = availableRewards.Where(r =>
                    r.MinWave <= wave && r.MaxWave >= wave && r.CostWavePoints <= wavePointsLeft && (!r.IsExclusive || !r.IsAlreadyHave()))
                .ToList();

            float weigthSum = tmpRewards.Sum(r => r.Weight);
            float rndSum = Random.Range(0f, weigthSum);
            float curWeigth = 0;
            RewardConfig reward = null;
            for (int i = 0; i < tmpRewards.Count; i++) {
                curWeigth += tmpRewards[i].Weight;
                if (rndSum < curWeigth) {
                    reward = tmpRewards[i];
                    wavePointsLeft -= tmpRewards[i].CostWavePoints;
                    if (reward.IsExclusive) {
                        availableRewards.Remove(reward);
                    }

                    break;
                }
            }

            rewards.Add(new Reward() {
                Type = reward.Type,
                Amount = 1,
                Data = reward.GetData()
            });
        }

        return rewards;
    }
}