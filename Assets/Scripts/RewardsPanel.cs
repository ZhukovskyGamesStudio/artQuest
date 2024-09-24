using System.Collections.Generic;
using UnityEngine;

public class RewardsPanel : MonoBehaviour {
    [SerializeField]
    private RewardView _rewardViewPrefab;

    [SerializeField]
    private Transform _rewardsGrid;

    [SerializeField]
    private Animation _animation;

    private List<RewardView> _views = new List<RewardView>();

    public void SetRewardsWithAnimation(List<Reward> rewards, bool isAdd) {
        _animation.Play(isAdd ? "AddRewards" : "LoseRewards");
        SetRewards(rewards);
    }

    public void SetRewards(List<Reward> rewards) {
        foreach (var VARIABLE in _views) {
            Destroy(VARIABLE.gameObject);
        }

        _views = new List<RewardView>();

        foreach (var VARIABLE in rewards) {
            RewardView rewardView = Instantiate(_rewardViewPrefab, _rewardsGrid);
            rewardView.SetData(VARIABLE);
            _views.Add(rewardView);
        }
    }
}