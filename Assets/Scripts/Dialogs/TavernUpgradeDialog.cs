using System;

public class TavernUpgradeDialog : DialogBase {
    public override DialogType Type() => DialogType.TavernUpgradeDialog;

    public override Type DataType() => typeof(TavernUpgradeDialogData);
}

public class TavernUpgradeDialogData : DialogData {
    public override DialogType Type() => DialogType.TavernUpgradeDialog;
}