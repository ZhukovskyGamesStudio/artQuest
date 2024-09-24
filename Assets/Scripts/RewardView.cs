using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardView : MonoBehaviour {
    [SerializeField]
    private TMP_Text _nameText, _amountText;

    [SerializeField]
    private Image _icon;

    [SerializeField]
    private Sprite _coin, _color, _shablon;

    public void SetData(Reward reward) {
        _nameText.text = GetNameByReward(reward);
        _amountText.text = GetAmountByReward(reward);
        SetIconByReward(reward);
    }

    private string GetNameByReward(Reward reward) {
        switch (reward.Type) {
            case RewardType.Coin:
                return "Coin" + (reward.Amount > 1 ? "s" : "");
            case RewardType.Pixel:
                return reward.Data + " pixel" + (reward.Amount > 1 ? "s" : "");
            case RewardType.Shablon:
                return ShablonsTable.Shablons[int.Parse(reward.Data)].Name + " shablon";
        }

        throw new ArgumentException($"Unknown reward type {reward.Type}");
    }

    private string GetAmountByReward(Reward reward) {
        switch (reward.Type) {
            case RewardType.Coin:
            case RewardType.Pixel:
                return reward.Amount.ToString();
            case RewardType.Shablon:
                return string.Empty;
        }

        throw new ArgumentException($"Unknown reward type {reward.Type}");
    }

    private void SetIconByReward(Reward reward) {
        switch (reward.Type) {
            case RewardType.Coin:
                _icon.sprite = _coin;
                //TODO Add changing icon by amount
                return;

            case RewardType.Pixel:
                _icon.sprite = _color;
                _icon.color = ColorsTable.ColorByName(reward.Data);
                return;
            case RewardType.Shablon:
                int index = int.Parse(reward.Data);
                _icon.sprite = ShablonsTable.Shablons[index].Sprite;
                return;
        }

        throw new ArgumentException($"Unknown reward type {reward.Type}");
    }
}