using UnityEngine;
using UnityEngine.SceneManagement;

public class AdminMainPanel : MonoBehaviour {
    public void ResetGame() {
        DdolManager.ClearInstance();
        SceneManager.LoadScene("LoadingScene");
    }
}