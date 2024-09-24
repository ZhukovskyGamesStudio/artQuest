using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WavesConfig", menuName = "ScriptableObjects/WavesConfig", order = 5)]
public class WavesConfig : ScriptableObject {
    public List<WaveData> _waves;
    public float MultiplierOnEndWaveCycle;

    public WaveData GetWaveDataByIndex(int index) {
        int cycleMultiplier = index / _waves.Count;
        index %= _waves.Count;
        WaveData wave = new WaveData() {
            Points = _waves[index].Points,
            PossibleEnemies = _waves[index].PossibleEnemies
        };
        wave.Points *= Mathf.FloorToInt(cycleMultiplier * MultiplierOnEndWaveCycle);
        return _waves[index];
    }
}

[Serializable]
public class WaveData {
    public int Points;
    public int DifficultyPoints = 1;
    public List<EnemyConfig> PossibleEnemies;
    public bool IsChangingBackground;
    public Sprite BackgroundSprite;
    public string BackgroundName;
}