using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShablonView : MonoBehaviour {
    [SerializeField]
    private TMP_Text _nameText;

    [SerializeField]
    private Image _icon;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private GameObject _closedPanel;

    public void SetData(string name, Sprite icon, bool isOpen, Action onSelect) {
        _nameText.text = isOpen ? name : "Not found";
        _icon.sprite = icon;
        _closedPanel.SetActive(!isOpen);
        _button.interactable = isOpen;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(delegate { onSelect?.Invoke(); });
    }
}