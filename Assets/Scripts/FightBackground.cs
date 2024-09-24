using TMPro;
using UnityEngine;

public class FightBackground : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private TMP_Text _nameText;

    public void SetData(Sprite sprite, string nameText) {
        _spriteRenderer.sprite = sprite;
        _nameText.text = nameText;
    }
}