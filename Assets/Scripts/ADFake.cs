using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ADFake : MonoBehaviour {
    [SerializeField]
    private Slider _progressBar;

    private float _adLength;
    private Action _onAdComplete;

    public void StartAd(float seconds, Action onComplete) {
        _adLength = seconds;
        _progressBar.maxValue = _adLength;
        StartCoroutine(Ad(onComplete));
    }

    private IEnumerator Ad(Action onComplete) {
        float curTime = 0;
        while (curTime <= _adLength) {
            curTime += Time.deltaTime;
            _progressBar.SetValueWithoutNotify(curTime);
            yield return new WaitForEndOfFrame();
        }

        gameObject.SetActive(false);
        onComplete?.Invoke();
        Destroy(gameObject, 0.2f);
    }
}