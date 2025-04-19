using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndFightPanel : MonoBehaviour {
    private const int PERCENT = 50;

    [SerializeField]
    private TMP_Text _lostOrWinText, _explanationText;

    [SerializeField]
    private RewardsPanel _withAdRewardsPanel, _rewardsPanel;

    [SerializeField]
    private AdManager _adManager;

    //private bool _isWin;

    private List<Reward> _rewards, _allRewards;

    public void OpenLoseState(List<Reward> rewards, List<Reward> allRewards) {
        gameObject.SetActive(true);
        //_isWin = false;
        _lostOrWinText.text = "Defeat";
        _explanationText.text = $"You get only {PERCENT}% of rewards";
        _rewards = rewards;
        _allRewards = allRewards;
        _rewardsPanel.SetRewards(rewards);

        _withAdRewardsPanel.gameObject.SetActive(true);
        _withAdRewardsPanel.SetRewards(allRewards);
    }

    public void OpenLeaveState(List<Reward> rewards) {
        gameObject.SetActive(true);
        //_isWin = true;
        _lostOrWinText.text = "Escaped";
        _explanationText.text = "You get all rewards";
        _rewards = rewards;
        _rewardsPanel.SetRewards(rewards);
        _withAdRewardsPanel.gameObject.SetActive(false);
    }

    public void OpenWinState(List<Reward> rewards) {
        gameObject.SetActive(true);
        //_isWin = true;
        _lostOrWinText.text = "Victory!";
        _explanationText.text = "You fought fabulously!";
        _rewards = rewards;
        _rewardsPanel.SetRewards(rewards);
        _withAdRewardsPanel.gameObject.SetActive(false);
    }

    public void CollectAndGo() {
        MetaCore.Instance.Inventory.AddRewards(_rewards);
        MetaCore.Instance.Party.Clear();
        SceneManager.LoadScene("MetaScene");
    }

    public void WatchAd() {
        _withAdRewardsPanel.gameObject.SetActive(false);
        _adManager.WatchAd(OnAdWatched);
    }

    private void OnAdWatched() {
        _rewards = _allRewards;
        _rewardsPanel.SetRewardsWithAnimation(_rewards, true);
    }
}