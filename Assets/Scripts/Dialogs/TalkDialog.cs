using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalkDialog : DialogBase {
    [SerializeField]
    private TMP_Text _text;

    [SerializeField]
    private Image _iconImage;

    [SerializeField]
    private Sprite _mainCharacterSprite, _mainEvilGuySprite;

    public override DialogType Type() => DialogType.TalkDialog;

    public override Type DataType() => typeof(TalkDialogData);

    public override void Init(DialogData data) {
        base.Init(data);
        TalkDialogData talkData = data as TalkDialogData;
        SetText(talkData.Text);
        SetIcon(talkData.IconType);
    }

    private void SetText(string text) {
        _text.text = text;
    }

    private void SetIcon(TalkIconType type) {
        _iconImage.sprite = type switch {
            TalkIconType.MainCharacter => _mainCharacterSprite,
            TalkIconType.MainEvilGuy => _mainEvilGuySprite,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}

public class TalkDialogData : DialogData {
    public TalkIconType IconType;
    public string Text;
    public override DialogType Type() => DialogType.TalkDialog;
}

public enum TalkIconType {
    None = 0,
    MainCharacter,
    MainEvilGuy
}