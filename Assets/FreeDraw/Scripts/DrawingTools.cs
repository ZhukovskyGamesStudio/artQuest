using DefaultNamespace;
using UnityEngine;

namespace FreeDraw.Scripts {
    // Helper methods used to set drawing settings
    public class DrawingTools {
        public static bool isCursorOverUI = false;
        private Color32 _rememberedColour;
        public float Transparency = 1f;

        public static Color32 TransparentColor => new Color(255f, 255f, 255f, 0f);

        // Changing pen settings is easy as changing the static properties Drawable.Pen_Colour and Drawable.Pen_Width
        public void SetMarkerColour(Color new_color) {
            Drawable.Pen_Colour = new_color;
        }

        // new_width is radius in pixels
        public void SetMarkerWidth(int new_width) {
            Drawable.Pen_Width = new_width;
        }

        public void SetTransparency(float amount) {
            Transparency = amount;
            Color c = Drawable.Pen_Colour;
            c.a = amount;
            Drawable.Pen_Colour = c;
        }

        public void SetEraser() {
            if (Drawable.Pen_Colour.a != TransparentColor.a) {
                _rememberedColour = Drawable.Pen_Colour;
            }

            SetMarkerWidth(0);
            SetMarkerColour(TransparentColor);
        }

        public void SetPen() {
            if (_rememberedColour.a != TransparentColor.a) {
                Drawable.Pen_Colour = _rememberedColour;
            }

            SetMarkerWidth(0);
        }

        public void SetBrush() {
            if (_rememberedColour.a != TransparentColor.a) {
                Drawable.Pen_Colour = _rememberedColour;
            }

            SetMarkerWidth(1);
        }

        public void ClearAllPixels() {
            DrawableAdapter.Drawable.ClearAll();
        }

        public void PartialSetEraser() {
            SetMarkerColour(new Color(255f, 255f, 255f, 0.5f));
        }
    }
}