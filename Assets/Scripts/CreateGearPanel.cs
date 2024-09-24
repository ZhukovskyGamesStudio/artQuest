using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateGearPanel : MonoBehaviour {
    [SerializeField]
    private TMP_Text _pixelsAmountText, _statsText, _costText;

    [SerializeField]
    private Button _createButton;

    [SerializeField]
    private TMP_InputField _nameInput;

    private int _coins, _drawnPixels;

    private ShablonConfig _currentShablon;
    private FighterStats _gearStats = new FighterStats();
    private int _powerPerPixel = 0;

    public void SetShablon(ShablonConfig sh, int coinsAmount) {
        _currentShablon = sh;
        _coins = coinsAmount;
        UpdateData(new Dictionary<Color32, int>());
    }

    public void UpdateData(Dictionary<Color32, int> pixels) {
        _drawnPixels = pixels.Values.Sum();
        _pixelsAmountText.text = $"Pixels: {_drawnPixels}/{_currentShablon.MinPixels}";
        _gearStats = RecalculateStats(pixels);

        if (_currentShablon is GearShablonConfig) {
            string statsString = _gearStats.GetString();
            _statsText.text = statsString;
        } else if (_currentShablon is DecorationShablonConfig decorConfig) {
            _statsText.text = string.Format(decorConfig.CustomStatsText, _powerPerPixel);
        }

        _createButton.interactable = IsCreateAvailable();
        _costText.text = $"Create for {GetCost()}";
    }

    private FighterStats RecalculateStats(Dictionary<Color32, int> pixels) {
        FighterStats stats = new FighterStats();
        if (_currentShablon is GearShablonConfig gearShablon) {
            stats.Add(gearShablon.Stats);
            foreach (var kvp in pixels) {
                for (int i = 0; i < kvp.Value; i++) {
                    FighterStats pixelStat = ColorsTable.ConfigByColor(kvp.Key).Stats;
                    stats.Add(pixelStat);
                }
            }
        } else if (_currentShablon is DecorationShablonConfig decorationShablonConfig) {
            int pixelsCount = 0;
            foreach (KeyValuePair<Color32, int> kvp in pixels) {
                pixelsCount += kvp.Value;
            }

            _powerPerPixel = Mathf.FloorToInt(pixelsCount * decorationShablonConfig.PowerPerPixel * 100);
        }

        return stats;
    }

    private bool IsCreateAvailable() {
        return _coins >= _currentShablon.CostPerPixel * _drawnPixels && _drawnPixels >= _currentShablon.MinPixels &&
               _nameInput.text != string.Empty;
    }

    public FighterStats GetStats() => _gearStats;
    public int GetCost() => Mathf.FloorToInt(_currentShablon.CostPerPixel * _drawnPixels);
}