using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearView : MonoBehaviour {
    [SerializeField]
    private TMP_Text _nameText;

    [SerializeField]
    private Image _icon, _lockedIcon;

    [SerializeField]
    private GameObject _lock;

    [SerializeField]
    private Button _button;

    public GearType GearType;

    private Fighter _fighter;
    private bool _isLocked;

    public void SetData(string name, Sprite icon, GearType type, Action<GearView> onSelect) {
        _nameText.text = name;
        _icon.sprite = icon;
        GearType = type;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(delegate {
            if (_isLocked) {
                return;
            }

            onSelect?.Invoke(this);
        });
    }

    public void CloseLock(Fighter fighter) {
        _lock.SetActive(true);
        _lockedIcon.sprite = fighter.GetClassIcon;
        _fighter = fighter;
        //_isLocked = true;
    }

    public void OpenLock() {
        if (_fighter == null) {
            return;
        }

        _fighter.RemoveGear(GearType);
        _lock.SetActive(false);
        _isLocked = false;
    }
}