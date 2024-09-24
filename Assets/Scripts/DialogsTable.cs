using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogsTable : InitableManager {
    public static DialogsTable Instance;

    [SerializeField]
    private List<DialogBase> _dialogs;

    public DialogBase GetDialogByType(DialogType type) => _dialogs.FirstOrDefault(d => d.Type() == type);

    public override IEnumerator Init() {
        Instance = this;
        return base.Init();
    }
}