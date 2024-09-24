using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DecorationGridView : MonoBehaviour {
    [SerializeField]
    private PlacedDecoration _decorationPrefab;

    [SerializeField]
    private GameObject _placedIndicator;

    [SerializeField]
    private EventTrigger _eventTrigger;

    [SerializeField]
    private Image _image;

    private Decoration _decoration;
    private Action<Decoration, Vector3> _onEndDrag;
    private Action<Decoration> _onRemove;
    private Transform _tavernDecorationsParent;

    public void SetData(Transform decorationsParent, Decoration decoration, Action<Decoration, Vector3> onEndDrag,
        Action<Decoration> onRemove) {
        _tavernDecorationsParent = decorationsParent;
        _decoration = decoration;
        _onEndDrag = onEndDrag;
        _onRemove = onRemove;
        _image.sprite = decoration.Sprite;
    }

    public void SetPlaced(bool isPlaced) {
        _placedIndicator.SetActive(isPlaced);
        _eventTrigger.enabled = !isPlaced;
    }

    public void OnBeginDrag() {
        PlacedDecoration placed = Instantiate(_decorationPrefab, Input.mousePosition, Quaternion.identity, _tavernDecorationsParent);
        placed.Init(_decoration, _onEndDrag, _onRemove);
    }
}