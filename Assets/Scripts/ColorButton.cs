using FreeDraw.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour {
    [SerializeField]
    private TMP_Text _amountText;

    [SerializeField]
    private Button _button;

    private int _amount = 0;

    private Color32 _color;

    private void Awake() {
        _button.onClick.RemoveListener(ChangePixelColor());
        _button.onClick.AddListener(ChangePixelColor());
    }

    private UnityAction ChangePixelColor() {
        return delegate { Drawable.Pen_Colour = _color; };
    }

    public void SetData(Color32 newColor, int amount) {
        _color = newColor;
        _amount = amount;
        SetColor();
        UpdateAmount();
    }

    public void ChangeAmount(int newAmount) {
        _amount = newAmount;
        UpdateAmount();
    }

    private void SetColor() {
        _button.targetGraphic.color = _color;
    }

    private void UpdateAmount() {
        _amountText.text = _amount.ToString();
        _button.interactable = _amount != 0;
    }
}