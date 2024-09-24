using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminPanelManager : MonoBehaviour {
    [SerializeField]
    private AdminPanelConfig _adminPanelConfig;

    [SerializeField]
    private RewardsConfig _rewardsConfig;

    [SerializeField]
    private Button _openAdminPanelButton;

    private float timeSpeed = 1;

    private void Awake() {
        /*
#if !UNITY_EDITOR
    Destroy(gameObject);
#endif*/

#if UNITY_EDITOR
        ActivateAdminPanelButton();
#endif
    }

    private void Start() {
        Inventory.OnNewMaxLevelReached += TryEnableAdminPanel;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.RightBracket)) {
            SpeedUp();
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket)) {
            SpeedDown();
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            GiveManyRewards();
        }
    }

    public void SpeedUp() {
        timeSpeed *= 2;
        if (timeSpeed > _adminPanelConfig.MaxGameSpeed) {
            timeSpeed = _adminPanelConfig.MaxGameSpeed;
        }

        Time.timeScale = timeSpeed;
    }

    public void SpeedDown() {
        timeSpeed /= 2;
        if (timeSpeed < _adminPanelConfig.MinGameSpeed) {
            timeSpeed = _adminPanelConfig.MinGameSpeed;
        }

        Time.timeScale = timeSpeed;
    }

    public void GiveManyRewards() {
        RewardGenerator rewG = new RewardGenerator(_rewardsConfig);
        for (int i = 0; i < 25; i++) {
            List<Reward> rews = rewG.GenerateRewardForWave(i, 100);
            MetaCore.Instance.Inventory.AddRewards(rews);
        }
    }

    private void TryEnableAdminPanel(int maxLevelReached) {
        if (maxLevelReached > _adminPanelConfig.LevelToOpenAdminPanel) {
            ActivateAdminPanelButton();
        }
    }

    private void ActivateAdminPanelButton() {
        _openAdminPanelButton.gameObject.SetActive(true);
    }
}