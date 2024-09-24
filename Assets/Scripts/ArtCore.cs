using UnityEngine;

namespace DefaultNamespace {
    public class ArtCore : MonoBehaviour {
        public static ArtCore Instance;

        private void Awake() {
            Instance = this;
        }

        public void ShowArtMenu() {
            ArtMenuDialogData data = new ArtMenuDialogData() { };
            DialogManager.Instance.ShowDialog(data);
        }

        public void ShowDrawing() {
            DrawingDialogData data = new DrawingDialogData() { };
            DialogManager.Instance.ShowDialog(data);
        }
    }
}