using System;
using UnityEngine;

public class TavernMenuDialog : DialogBase {
    private Transform _placedDecorationsHolder;
    public override DialogType Type() => DialogType.TavernMenuDialog;

    public override Type DataType() => typeof(TavernMenuDialogData);

    public override void Init(DialogData data) {
        base.Init(data);
        TavernMenuDialogData menuData = data as TavernMenuDialogData;
        _placedDecorationsHolder = menuData.PlacedDecorationsHolder;
    }

    public void UpgradeTavern() {
        Close();
        TavernUpgradeDialogData data = new TavernUpgradeDialogData() { };
        DialogManager.Instance.ShowDialog(data);
    }

    public void DecorateTavern() {
        Close();
        DecorateTavernDialogData data = new DecorateTavernDialogData() {
            PlacedDecorationsHolder = _placedDecorationsHolder
        };
        DialogManager.Instance.ShowDialog(data);
    }

    public void ShowStatictics() {
        Close();
        throw new NotImplementedException();
    }
}

public class TavernMenuDialogData : DialogData {
    public Transform PlacedDecorationsHolder;
    public override DialogType Type() => DialogType.TavernMenuDialog;
}