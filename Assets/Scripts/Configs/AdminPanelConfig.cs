using UnityEngine;

[CreateAssetMenu(fileName = "AdminPanelConfig", menuName = "ScriptableObjects/AdminConfig", order = 0)]
public class AdminPanelConfig : ScriptableObject {
    public float MinGameSpeed, MaxGameSpeed;
    public int LevelToOpenAdminPanel = 20;
}