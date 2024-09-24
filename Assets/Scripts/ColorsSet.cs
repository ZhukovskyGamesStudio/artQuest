using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ColorsSet : MonoBehaviour {
    [SerializeField]
    private List<PixelConfig> _colors;

    [SerializeField]
    private ColorButton _colorButtonPrefab;

    [SerializeField]
    private Transform _colorGrid;

    public int DEFAULT_AMOUNT = 25;
    private Dictionary<Color32, int> _colorAmount = new Dictionary<Color32, int>();
    private Dictionary<Color32, ColorButton> _colorButtonsDictionary = new Dictionary<Color32, ColorButton>();
    Dictionary<Color32, int> HaveColors => MetaCore.Instance.Inventory.Colors;

    private void Awake() {
        DrawableAdapter.Drawable.OnPixelDrawn += RemovePixel;
        DrawableAdapter.Drawable.OnPixelErased += AddPixel;
    }

    private void Start() {
        InitPixelButtons();
    }

    private void InitPixelButtons() {
        foreach (var kvp in _colorButtonsDictionary) {
            Destroy(kvp.Value.gameObject);
        }

        _colorButtonsDictionary = new Dictionary<Color32, ColorButton>();
        _colorAmount = new Dictionary<Color32, int>();
        foreach (var color in _colors) {
            Color32 color32 = color.Color;
            if (!HaveColors.ContainsKey(color32)) {
                continue;
            }

            ColorButton colorButton = Instantiate(_colorButtonPrefab, _colorGrid);
            colorButton.SetData(color32, HaveColors[color32]);

            _colorButtonsDictionary.Add(color32, colorButton);
            _colorAmount.Add(color32, HaveColors[color32]);
            if (!DrawableAdapter.Drawable.CheckPixel.ContainsKey(color32)) {
                DrawableAdapter.Drawable.CheckPixel.Add(color32, CheckPixel);
            }
        }
    }

    public void ResetPixelAmounts() {
        List<Color32> keys = new List<Color32>(_colorAmount.Keys);
        foreach (Color32 color32 in keys) {
            if (!HaveColors.ContainsKey(color32)) {
                _colorAmount[color32] = 0;
                _colorButtonsDictionary[color32].ChangeAmount(0);
            } else {
                _colorAmount[color32] = HaveColors[color32];
                _colorButtonsDictionary[color32].ChangeAmount(HaveColors[color32]);
            }
        }
    }

    public bool CheckPixel(Color32 color32) {
        if (!_colorAmount.ContainsKey(color32)) {
            return true;
        }

        return _colorAmount[color32] > 0;
    }

    public void AddPixel(Color32 color32) {
        if (!_colorAmount.ContainsKey(color32)) {
            return;
        }

        _colorAmount[color32]++;
        _colorButtonsDictionary[color32].ChangeAmount(_colorAmount[color32]);
    }

    public void RemovePixel(Color32 color32) {
        if (!_colorAmount.ContainsKey(color32)) {
            return;
        }

        _colorAmount[color32]--;
        _colorButtonsDictionary[color32].ChangeAmount(_colorAmount[color32]);
    }
}