using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HireDialog : DialogBase {
    [SerializeField]
    private TMP_Text _nameText, _classText, _statsText, _costText;

    [SerializeField]
    private Image _icon;

    private int _cost;
    private HireDialogData _hireDialogData;
    public override DialogType Type() => DialogType.HireDialog;

    public override Type DataType() => typeof(HireDialogData);

    public override void Init(DialogData data) {
        base.Init(data);
        _hireDialogData = data as HireDialogData;
        SetNames();
        SetStats();
        SetCost(_hireDialogData.Cost);
    }

    private void SetNames() {
        _classText.text = _hireDialogData.ClassType.ToString();
        _nameText.text = _hireDialogData.Name;
        _icon.sprite = ClassesTable.Instance.GetConfigByType(_hireDialogData.ClassType).Icon64;
    }

    private void SetStats() {
        _statsText.text = _hireDialogData.Stats.GetString();
    }

    private void SetCost(int cost) {
        _cost = cost;
        _costText.text = cost > 0 ? cost.ToString() : "Free";
    }

    public void Hire() {
        if (MetaCore.Instance.Inventory.Coins < _cost) {
            ErrorDialogData data = new ErrorDialogData() {
                Text = "Вам не хватает монет!"
            };
            DialogManager.Instance.ShowDialog(data);
            return;
        }

        if (!MetaCore.Instance.Party.HasEmptyPlace) {
            ErrorDialogData data = new ErrorDialogData() {
                Text = "Ошибка. В вашей команде нет свободного места!"
            };
            DialogManager.Instance.ShowDialog(data);
            return;
        }

        PartyMember member = new PartyMember() {
            ClassType = _hireDialogData.ClassType,
            Name = _hireDialogData.Name,
            Stats = _hireDialogData.Stats,
            Sword = null,
            Helmet = null,
        };
        _hireDialogData.OnHired?.Invoke(member);
        MetaCore.Instance.Party.AddMember(member);
        MetaCore.Instance.Inventory.AddCoin(-_cost);
        Close();
    }
}

public class HireDialogData : DialogData {
    public FighterClassType ClassType;
    public int Cost;
    public string Name;
    public Action<PartyMember> OnHired;
    public FighterStats Stats;
    public override DialogType Type() => DialogType.HireDialog;
}