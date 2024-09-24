using System;

[Serializable]
public class Decoration : DrawableItem {
    public DecorationType Type;

    public float PowerPerPixel;
    //TODO add diff upgrades to diff decorations
}