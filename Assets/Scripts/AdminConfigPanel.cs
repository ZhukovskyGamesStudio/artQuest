using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class AdminConfigPanel : MonoBehaviour {
    [SerializeField]
    private WavesConfig _wavesConfig;

    [SerializeField]
    private AdminWaveConfigLineView _waveLine;

    [SerializeField]
    private Transform _wavesHolder;

    private List<AdminWaveConfigLineView> _waveLines;

    private void Awake() {
        _waveLines = new List<AdminWaveConfigLineView>();
        foreach (var VARIABLE in _wavesConfig._waves) {
            AdminWaveConfigLineView a = Instantiate(_waveLine, _wavesHolder);
            _waveLines.Add(a);
        }
    }

    private void OnEnable() {
        SetTableFromConfig();
    }

    private void OnDisable() {
        SetConfigFromTable();
    }

    private void SetTableFromConfig() {
        for (int index = 0; index < _wavesConfig._waves.Count; index++) {
            WaveData VARIABLE = _wavesConfig._waves[index];
            _waveLines[index].Set(VARIABLE, index);
        }
    }

    private void SetConfigFromTable() {
        for (int index = 0; index < _wavesConfig._waves.Count; index++) {
            _wavesConfig._waves[index] = _waveLines[index].Get();
        }
    }
}