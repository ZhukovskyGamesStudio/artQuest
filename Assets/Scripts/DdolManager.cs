using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DdolManager : MonoBehaviour {
    private void OnFirstInit() {
        StartCoroutine(InitChildManagers());
    }

    private IEnumerator InitChildManagers() {
        foreach (InitableManager initableManager in _managersToInit.Where(m => m.gameObject.activeSelf)) {
            yield return StartCoroutine(initableManager.Init());
        }

        yield return StartCoroutine(LoadScene("MetaScene"));
    }

    private IEnumerator LoadScene(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);
        bool isLoading = true;

        void OnSceneLoadedAsync(Scene arg0, LoadSceneMode arg1) {
            isLoading = false;
            SceneManager.sceneLoaded -= OnSceneLoadedAsync;
        }

        SceneManager.sceneLoaded += OnSceneLoadedAsync;
        while (isLoading) {
            yield return new WaitForEndOfFrame();
        }
    }

    #region Instance

    private static DdolManager Instance;

    [SerializeField]
    private List<InitableManager> _managersToInit = new List<InitableManager>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            OnFirstInit();
        } else {
            Destroy(gameObject);
        }
    }

    public static void ClearInstance() {
        Destroy(Instance.gameObject);
        Instance = null;
    }

    #endregion
}