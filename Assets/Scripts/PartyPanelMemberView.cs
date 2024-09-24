using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyPanelMemberView : MonoBehaviour {
    [SerializeField]
    private TMP_Text _name;

    [SerializeField]
    private Image _icon;

    [SerializeField]
    private Sprite _emptySprite;

    [SerializeField]
    private GameObject _removeButton;

    public bool IsEmpty = true;

    public void Set(PartyMember member) {
        _name.text = member.Name;
        _icon.sprite = ClassesTable.Instance.GetConfigByType(member.ClassType).Icon64;
        _removeButton.SetActive(true);
        IsEmpty = false;
    }

    public void Clear() {
        _name.text = "";
        _icon.sprite = _emptySprite;
        _removeButton.SetActive(false);
        IsEmpty = true;
    }
}