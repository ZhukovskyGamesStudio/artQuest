using System;
using UnityEngine;

public class AdManager : MonoBehaviour {
    [SerializeField]
    private ADFake _adFakePrefab;

    [SerializeField]
    private float _fakeAdLength = 15f;

    private ADFake _curAd = null;

    public void WatchAd(Action onComplete) {
        if (_curAd != null) {
            return;
        }

        _curAd = Instantiate(_adFakePrefab);
        _curAd.StartAd(_fakeAdLength, onComplete);
    }
}