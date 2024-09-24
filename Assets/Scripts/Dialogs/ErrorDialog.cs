using System;
using TMPro;
using UnityEngine;

public class ErrorDialog : DialogBase {
    [SerializeField]
    private TMP_Text _text;

    public override DialogType Type() => DialogType.ErrorDialog;

    public override Type DataType() => typeof(ErrorDialogData);

    public override void Init(DialogData data) {
        base.Init(data);
        ErrorDialogData errorData = data as ErrorDialogData;

        _text.SetText(errorData.Text);
    }
}

public class ErrorDialogData : DialogData {
    public string Text;
    public override DialogType Type() => DialogType.ErrorDialog;
}