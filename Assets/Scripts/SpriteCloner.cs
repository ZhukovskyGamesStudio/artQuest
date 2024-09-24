using UnityEngine;

public static class SpriteCloner {
    public static Sprite CloneSprite(Sprite source) {
// Create a copy of the texture by reading and applying the raw texture data.
        Texture2D textureCloned = CloneTexture(source.texture);
        textureCloned.filterMode = source.texture.filterMode;
        //textureCloned.alphaIsTransparency = source.texture.alphaIsTransparency;
        Sprite sprite = Sprite.Create(textureCloned, new Rect(0, 0, textureCloned.width, textureCloned.height), new Vector2(0.5f, 0.5f),
            source.pixelsPerUnit);
        return sprite;
    }

    private static Texture2D CloneTexture(Texture2D source) {
// Create a copy of the texture by reading and applying the raw texture data.
        Texture2D texCopy = new Texture2D(source.width, source.height, source.format, source.mipmapCount > 1);
        var newData = source.GetRawTextureData<Color32>();
// Load the original texture data
        texCopy.LoadRawTextureData<Color32>(newData);
        texCopy.Apply();
        return texCopy;
    }
}