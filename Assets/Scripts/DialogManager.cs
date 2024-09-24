using System.Collections;

public class DialogManager : InitableManager {
    public static DialogManager Instance;

    public override IEnumerator Init() {
        Instance = this;
        return base.Init();
    }

    public void ShowDialog(DialogData data) {
        DialogBase dialogShablon = DialogsTable.Instance.GetDialogByType(data.Type());
        DialogBase dialog = Instantiate(dialogShablon, transform);
        dialog.Init(data);
    }
}