using FreeDraw.Scripts;
using UnityEngine;

namespace DefaultNamespace {
    public class DrawableAdapter : MonoBehaviour {
        private static DrawableAdapter Instance;

        [SerializeField]
        private SpriteRenderer _shablonBack, _shablonFront;

        [SerializeField]
        private Drawable _drawable;

        [SerializeField]
        private Camera _drawableCamera;

        public DrawingTools _tools;
        int depth = 24;
        int height = 256;
        int width = 256;

        public static DrawingTools Tools => Instance._tools;
        public static Drawable Drawable => Instance._drawable;
        public static Camera DrawableCamera => Instance._drawableCamera;
        public static Sprite GetCameraView => Instance.CaptureScreen();

        private void Awake() {
            Instance = this;
            _tools = new DrawingTools();
        }

        public static void SetShablonView(Sprite sprite) {
            Instance._shablonBack.sprite = sprite;
            Instance._shablonFront.sprite = sprite;
        }

        public static void SetDrawableActive(bool isOn) {
            Drawable.gameObject.SetActive(isOn);
        }

//method to render from camera
        public Sprite CaptureScreen() {
            RenderTexture renderTexture = new RenderTexture(width, height, depth);
            Rect rect = new Rect(0, 0, width, height);
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

            _drawableCamera.targetTexture = renderTexture;
            _drawableCamera.Render();

            RenderTexture currentRenderTexture = RenderTexture.active;
            RenderTexture.active = renderTexture;
            texture.ReadPixels(rect, 0, 0);
            texture.Apply();

            _drawableCamera.targetTexture = null;
            RenderTexture.active = currentRenderTexture;
            Destroy(renderTexture);

            Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

            return sprite;
        }
    }
}