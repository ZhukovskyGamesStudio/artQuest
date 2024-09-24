using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FreeDraw.Scripts {
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))] // REQUIRES A COLLIDER2D to function
    // 1. Attach this to a read/write enabled sprite image
    // 2. Set the drawing_layers  to use in the raycast
    // 3. Attach a 2D collider (like a Box Collider 2D) to this sprite
    // 4. Hold down left mouse to draw on this texture!
    public class Drawable : MonoBehaviour {
        public delegate void Brush_Function(Vector2 world_position);

        // PEN COLOUR
        public static Color Pen_Colour = Color.red; // Change these to change the default drawing settings

        // PEN WIDTH (actually, it's a radius, in pixels)
        public static int Pen_Width = 0;

        public LayerMask Drawing_Layers;

        public bool Reset_Canvas_On_Play = true;

        // The colour the canvas is reset to each time
        public Color Reset_Colour = DrawingTools.TransparentColor; // By default, reset the canvas to be transparent
        public Dictionary<Color32, Func<Color32, bool>> CheckPixel = new Dictionary<Color32, Func<Color32, bool>>();
        Color[] clean_colours_array;
        Color32[] cur_colors;

        // This is the function called when a left click happens
        // Pass in your own custom one to change the brush type
        // Set the default function in the Awake method
        public Brush_Function current_brush;

        // MUST HAVE READ/WRITE enabled set in the file editor of Unity
        Sprite drawable_sprite;
        Texture2D drawable_texture;
        bool mouse_was_previously_held_down = false;
        bool no_drawing_on_current_drag = false;

        // Used to reference THIS specific file without making all methods static
        public Action<Color32> OnPixelDrawn, OnPixelErased;

        Vector2 previous_drag_position;
        Sprite shablon;
        Color transparent;

        private void Awake() {
            // DEFAULT BRUSH SET HERE
            current_brush = PenBrush;

            // Should we reset our canvas image when we hit play in the editor?
            if (Reset_Canvas_On_Play)
                ResetCanvas();
        }
//////////////////////////////////////////////////////////////////////////////

        // This is where the magic happens.
        // Detects when user is left clicking, which then call the appropriate function
        void Update() {
            if (drawable_sprite == null) {
                return;
            }

            // Is the user holding down the left mouse button?
            bool mouse_held_down = Input.GetMouseButton(0);
            if (mouse_held_down && !no_drawing_on_current_drag) {
                // Convert mouse coordinates to world coordinates
                Vector2 mouse_world_position = DrawableAdapter.DrawableCamera.ScreenToWorldPoint(Input.mousePosition);

                // Check if the current mouse position overlaps our image
                Collider2D hit = Physics2D.OverlapPoint(mouse_world_position, Drawing_Layers.value);
                if (hit != null && hit.transform != null) {
                    // We're over the texture we're drawing on!
                    // Use whatever function the current brush is
                    current_brush(mouse_world_position);
                } else {
                    // We're not over our destination texture
                    previous_drag_position = Vector2.zero;
                    if (!mouse_was_previously_held_down) {
                        // This is a new drag where the user is left clicking off the canvas
                        // Ensure no drawing happens until a new drag is started
                        no_drawing_on_current_drag = true;
                    }
                }
            }
            // Mouse is released
            else if (!mouse_held_down) {
                previous_drag_position = Vector2.zero;
                no_drawing_on_current_drag = false;
            }

            mouse_was_previously_held_down = mouse_held_down;
        }

//////////////////////////////////////////////////////////////////////////////
// BRUSH TYPES. Implement your own here

        // When you want to make your own type of brush effects,
        // Copy, paste and rename this function.
        // Go through each step
        public void BrushTemplate(Vector2 world_position) {
            // 1. Change world position to pixel coordinates
            Vector2 pixel_pos = WorldToPixelCoordinates(world_position);

            // 2. Make sure our variable for pixel array is updated in this frame
            cur_colors = drawable_texture.GetPixels32();

            ////////////////////////////////////////////////////////////////
            // FILL IN CODE BELOW HERE

            // Do we care about the user left clicking and dragging?
            // If you don't, simply set the below if statement to be:
            //if (true)

            // If you do care about dragging, use the below if/else structure
            if (previous_drag_position == Vector2.zero) {
                // THIS IS THE FIRST CLICK
                // FILL IN WHATEVER YOU WANT TO DO HERE
                // Maybe mark multiple pixels to colour?
                MarkPixelsToColour(pixel_pos, Pen_Width, Pen_Colour);
            } else {
                // THE USER IS DRAGGING
                // Should we do stuff between the previous mouse position and the current one?
                ColourBetween(previous_drag_position, pixel_pos, Pen_Width, Pen_Colour);
            }
            ////////////////////////////////////////////////////////////////

            // 3. Actually apply the changes we marked earlier
            // Done here to be more efficient
            ApplyMarkedPixelChanges();

            // 4. If dragging, update where we were previously
            previous_drag_position = pixel_pos;
        }

        // Default brush type. Has width and colour.
        // Pass in a point in WORLD coordinates
        // Changes the surrounding pixels of the world_point to the static pen_colour
        public void PenBrush(Vector2 world_point) {
            Vector2 pixel_pos = WorldToPixelCoordinates(world_point);

            cur_colors = drawable_texture.GetPixels32();

            if (previous_drag_position == Vector2.zero) {
                // If this is the first time we've ever dragged on this image, simply colour the pixels at our mouse position
                MarkPixelsToColour(pixel_pos, Pen_Width, Pen_Colour);
            } else {
                // Colour in a line from where we were on the last update call
                ColourBetween(previous_drag_position, pixel_pos, Pen_Width, Pen_Colour);
            }

            ApplyMarkedPixelChanges();

            //Debug.Log("Dimensions: " + pixelWidth + "," + pixelHeight + ". Units to pixels: " + unitsToPixels + ". Pixel pos: " + pixel_pos);
            previous_drag_position = pixel_pos;
        }

        // Helper method used by UI to set what brush the user wants
        // Create a new one for any new brushes you implement
        public void SetPenBrush() {
            // PenBrush is the NAME of the method we want to set as our current brush
            current_brush = PenBrush;
        }

        // Set the colour of pixels in a straight line from start_point all the way to end_point, to ensure everything inbetween is coloured
        public void ColourBetween(Vector2 start_point, Vector2 end_point, int width, Color color) {
            // Get the distance from start to finish
            float distance = Vector2.Distance(start_point, end_point);
            Vector2 direction = (start_point - end_point).normalized;

            Vector2 cur_position = start_point;

            // Calculate how many times we should interpolate between start_point and end_point based on the amount of time that has passed since the last update
            float lerp_steps = 1 / distance;

            for (float lerp = 0; lerp <= 1; lerp += lerp_steps) {
                cur_position = Vector2.Lerp(start_point, end_point, lerp);
                MarkPixelsToColour(cur_position, width, color);
            }
        }

        public void ClearAll() {
            if (drawable_sprite == null) {
                return;
            }

            MarkPixelsToColour(new Vector2(0, 0), 128, DrawingTools.TransparentColor);
            ApplyMarkedPixelChanges();
        }

        public void MarkPixelsToColour(Vector2 center_pixel, int pen_thickness, Color color_of_pen) {
            // Figure out how many pixels we need to colour in each direction (x and y)
            int center_x = (int)center_pixel.x;
            int center_y = (int)center_pixel.y;
            //int extra_radius = Mathf.Min(0, pen_thickness - 2);
            int minX = Mathf.Max(center_x - pen_thickness, 0);
            int maxX = Mathf.Min(center_x + pen_thickness, (int)drawable_sprite.rect.width - 1);

            int minY = Mathf.Max(center_y - pen_thickness, 0);
            int maxY = Mathf.Min(center_y + pen_thickness, (int)drawable_sprite.rect.height - 1);

            for (int x = minX; x <= maxX; x++) {
                for (int y = minY; y <= maxY; y++) {
                    Color32 beforeColor = GetPixelBeforeChange(x, y);
                    if (color_of_pen != DrawingTools.TransparentColor) {
                        if (!CheckPixel.ContainsKey(color_of_pen) || !CheckPixel[color_of_pen].Invoke(color_of_pen)) {
                            continue;
                        }

                        if (GetPixel(shablon, x, y).a == 0) {
                            continue;
                        }
                    }

                    OnPixelErased?.Invoke(beforeColor);
                    MarkPixelToChange(x, y, color_of_pen);
                    if (color_of_pen != DrawingTools.TransparentColor) {
                        OnPixelDrawn?.Invoke(color_of_pen);
                    }
                }
            }
        }

        public Color GetPixelBeforeChange(int x, int y) {
            return GetPixel(drawable_sprite, x, y);
        }

        private Color GetPixel(Sprite sprite, int x, int y) {
            Color32[] pixels32 = sprite.texture.GetPixels32();
            // Need to transform x and y coordinates to flat coordinates of array
            int array_pos = y * (int)sprite.rect.width + x;

            // Check if this is a valid position
            if (array_pos > pixels32.Length || array_pos < 0)
                throw new Exception($"Invalid pixel position in sprite {sprite.name}: {x},{y}");

            return pixels32[array_pos];
        }

        public void MarkPixelToChange(int x, int y, Color color) {
            // Need to transform x and y coordinates to flat coordinates of array
            int array_pos = y * (int)drawable_sprite.rect.width + x;

            // Check if this is a valid position
            if (array_pos > cur_colors.Length || array_pos < 0)
                return;

            cur_colors[array_pos] = color;
        }

        public void ApplyMarkedPixelChanges() {
            drawable_texture.SetPixels32(cur_colors);
            drawable_texture.Apply();
        }

        // Directly colours pixels. This method is slower than using MarkPixelsToColour then using ApplyMarkedPixelChanges
        // SetPixels32 is far faster than SetPixel
        // Colours both the center pixel, and a number of pixels around the center pixel based on pen_thickness (pen radius)
        public void ColourPixels(Vector2 center_pixel, int pen_thickness, Color color_of_pen) {
            // Figure out how many pixels we need to colour in each direction (x and y)
            int center_x = (int)center_pixel.x;
            int center_y = (int)center_pixel.y;
            //int extra_radius = Mathf.Min(0, pen_thickness - 2);

            for (int x = center_x - pen_thickness; x <= center_x + pen_thickness; x++) {
                for (int y = center_y - pen_thickness; y <= center_y + pen_thickness; y++) {
                    drawable_texture.SetPixel(x, y, color_of_pen);
                }
            }

            drawable_texture.Apply();
        }

        public Vector2 WorldToPixelCoordinates(Vector2 world_position) {
            // Change coordinates to local coordinates of this image
            Vector3 local_pos = transform.InverseTransformPoint(world_position);

            // Change these to coordinates of pixels
            float pixelWidth = drawable_sprite.rect.width;
            float pixelHeight = drawable_sprite.rect.height;
            float unitsToPixels = pixelWidth / drawable_sprite.bounds.size.x * transform.localScale.x;

            // Need to center our coordinates
            float centered_x = local_pos.x * unitsToPixels + pixelWidth / 2;
            float centered_y = local_pos.y * unitsToPixels + pixelHeight / 2;

            // Round current mouse position to nearest pixel
            Vector2 pixel_pos = new Vector2(Mathf.FloorToInt(centered_x), Mathf.FloorToInt(centered_y));

            return pixel_pos;
        }

        // Changes every pixel to be the reset colour
        public void ResetCanvas() {
            if (clean_colours_array == null) {
                return;
            }

            drawable_texture.SetPixels(clean_colours_array);
            drawable_texture.Apply();
        }

        private void InitCleanPixelsArray() {
            clean_colours_array = new Color[(int)drawable_sprite.rect.width * (int)drawable_sprite.rect.height];
            for (int x = 0; x < clean_colours_array.Length; x++)
                clean_colours_array[x] = Reset_Colour;
        }

        public void SetSprite(Sprite sprite) {
            GetComponent<SpriteRenderer>().sprite = sprite;
            drawable_sprite = sprite;
            drawable_texture = drawable_sprite.texture;
            drawable_texture.name = "texture" + Random.Range(0, 999);
            InitCleanPixelsArray();
        }

        public void SetShablon(Sprite sprite) => shablon = sprite;

        public Sprite GetSprite() => drawable_sprite;
    }
}