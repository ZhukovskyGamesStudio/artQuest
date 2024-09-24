using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class PlacedDecoration : MonoBehaviour {
        [SerializeField]
        private Button _removeButton;

        [SerializeField]
        private EventTrigger _eventTrigger;

        [SerializeField]
        private Image _image;

        private bool _canDrag;
        private Action<Decoration, Vector3> _onEndDrag;
        private Action<Decoration> _onRemove;

        private bool isDragging;
        public Decoration Decoration { get; private set; }

        private void Update() {
            if (!isDragging) {
                return;
            }

            Drag();

            if (Input.GetMouseButtonUp(0)) {
                EndDrag();
            }
        }

        public void Init(Decoration decor) {
            Decoration = decor;
            _image.sprite = decor.Sprite;
        }

        public void Init(Decoration decor, Action<Decoration, Vector3> onEndDrag, Action<Decoration> onRemove) {
            Decoration = decor;
            _image.sprite = decor.Sprite;
            _canDrag = true;
            BeginDrag();
            Subscribe(onEndDrag, onRemove);
            ShowRemoveButton(true);
        }

        public void BeginDrag() {
            isDragging = true;
        }

        public void Drag() {
            if (!_canDrag) {
                return;
            }

            transform.position = Input.mousePosition;
        }

        public void EndDrag() {
            isDragging = false;
            _onEndDrag?.Invoke(Decoration, transform.localPosition);
        }

        public void Remove() {
            _onRemove?.Invoke(Decoration);
            Destroy(gameObject);
        }

        public void ShowRemoveButton(bool isOn) {
            _removeButton.gameObject.SetActive(isOn);
            _eventTrigger.enabled = isOn;
        }

        public void Subscribe(Action<Decoration, Vector3> onEndDrag, Action<Decoration> onRemove) {
            _onEndDrag = onEndDrag;
            _onRemove = onRemove;
        }

        public void Unsubscribe() {
            _onEndDrag = null;
            _onRemove = null;
        }

        public void ChangeCanDrag(bool canDrag) {
            _canDrag = canDrag;
        }
    }
}