using System;

public class ArtMenuDialog : DialogBase {
    public override DialogType Type() => DialogType.ArtMenuDialog;

    public override Type DataType() => typeof(ArtMenuDialogData);

    public override void Init(DialogData data) {
        base.Init(data);
    }

    public void ShowInventory() {
        throw new NotImplementedException();
    }
}

public class ArtMenuDialogData : DialogData {
    public override DialogType Type() => DialogType.ArtMenuDialog;
}