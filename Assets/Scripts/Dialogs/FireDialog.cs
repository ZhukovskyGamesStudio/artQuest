using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireDialog : DialogBase {
    [SerializeField]
    private TMP_Text _classText, _nameText, _statsText;

    [SerializeField]
    private Image _icon;

    private int _memberIndex;
    private Action<int> _onFired;
    public override DialogType Type() => DialogType.FireDialog;

    public override Type DataType() => typeof(FireDialogData);

    public override void Init(DialogData data) {
        base.Init(data);
        FireDialogData fireData = data as FireDialogData;
        _statsText.text = MetaCore.Instance.Party.members[fireData.MemberIndex].Stats.GetString();
        _classText.text = fireData.ClassType.ToString();
        _nameText.text = fireData.Name;
        _icon.sprite = ClassesTable.Instance.GetConfigByType(fireData.ClassType).Icon64;
        _memberIndex = fireData.MemberIndex;
        _onFired = fireData.OnFired;
    }

    public void Fire() {
        _onFired?.Invoke(_memberIndex);
        MetaCore.Instance.Party.RemoveMember(_memberIndex);
        Close();
    }
}

public class FireDialogData : DialogData {
    public FighterClassType ClassType;
    public int MemberIndex;
    public string Name;
    public Action<int> OnFired;
    public override DialogType Type() => DialogType.FireDialog;
}