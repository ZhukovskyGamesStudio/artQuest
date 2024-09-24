using System.IO;
using UnityEngine;

public class PngSaver {
    public static void SaveSprite(Sprite sprite, string path) {
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }

        string fname = path + ".png";
        if (sprite.name.Contains("/")) {
            var index = sprite.name.LastIndexOf("/");
            var pt = path + sprite.name.Substring(0, index);
            if (!Directory.Exists(pt)) {
                Directory.CreateDirectory(pt);
            }

            fname = path + sprite.name + ".png";
        }

        byte[] bytes = ToPNG(sprite);
        File.WriteAllBytes(fname, bytes);
    }

    public void SaveSprite() {
        var sprites = Resources.LoadAll<Sprite>("main");
        var path = "D:/Splite/";
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }

        int counter = 0;
        foreach (var sprite in sprites) {
            string fname = path + sprite.name + ".png";
            if (sprite.name.Contains("/")) {
                var index = sprite.name.LastIndexOf("/");
                var pt = path + sprite.name.Substring(0, index);
                if (!Directory.Exists(pt)) {
                    Directory.CreateDirectory(pt);
                }

                fname = path + sprite.name + ".png";
            }

            byte[] bytes = ToPNG(sprite);
            File.WriteAllBytes(fname, bytes);
            ++counter;
            if (counter > 100) {
                break;
            }
        }
    }

    public static byte[] ToPNG(Sprite sprite) {
        Texture2D pSource = sprite.texture;
        Rect r = sprite.textureRect;
        int left = (int)r.x, top = (int)r.y, width = (int)r.width, height = (int)r.height;
        if (left < 0) {
            width += left;
            left = 0;
        }

        if (top < 0) {
            height += top;
            top = 0;
        }

        if (left + width > pSource.width) {
            width = pSource.width - left;
        }

        if (top + height > pSource.height) {
            height = pSource.height - top;
        }

        if (width <= 0 || height <= 0) {
            return null;
        }

        Color[] aSourceColor = pSource.GetPixels(0);

        //*** Make New
        Texture2D subtex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        //*** Make destination array
        int xLength = width * height;
        Color[] aColor = new Color[xLength];

        int i = 0;
        for (int y = 0; y < height; y++) {
            int sourceIndex = (y + top) * pSource.width + left;
            for (int x = 0; x < width; x++) {
                aColor[i++] = aSourceColor[sourceIndex++];
            }
        }

        //*** Set Pixels
        subtex.SetPixels(aColor);
        subtex.Apply();
        byte[] bytes = subtex.EncodeToPNG();
        return bytes;
    }
}