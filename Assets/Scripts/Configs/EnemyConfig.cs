using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObjects/EnemyConfig", order = 4)]
public class EnemyConfig : ScriptableObject {
    public int Cost = 1;
    public Sprite Sprite;
    public FighterStats Stats = new FighterStats();
    public float MinMultiplier = 0.5f, MaxMultiplier = 2f;

    public FighterStats GetStatsByDifficultyPoints(int points) {
        FighterStats stats = new FighterStats();
        stats.Add(Stats);
        stats.MultiplyRandom(points * MinMultiplier, points * MaxMultiplier);
        return stats;
    }
}