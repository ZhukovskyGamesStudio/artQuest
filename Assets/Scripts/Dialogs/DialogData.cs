public abstract class DialogData {
    public abstract DialogType Type();
}

public enum DialogType {
    None = 0,
    TalkDialog,
    ErrorDialog,
    HireDialog,
    FireDialog,
    TavernMenuDialog,
    TavernDecorateDialog,
    TavernUpgradeDialog,
    DrawingDialog,
    ArtMenuDialog
}