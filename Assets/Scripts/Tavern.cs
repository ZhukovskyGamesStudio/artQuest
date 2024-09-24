using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tavern {
    public Dictionary<Decoration, Vector3> PlacedDecorations = new Dictionary<Decoration, Vector3>();
    public static Tavern Instance => MetaCore.Instance.Tavern;

    public void InitEmpty() {
        PlacedDecorations = new Dictionary<Decoration, Vector3>();
    }

    public void MoveDecoration(Decoration decor, Vector3 newPos) {
        if (PlacedDecorations.ContainsKey(decor)) {
            PlacedDecorations[decor] = newPos;
        } else {
            PlacedDecorations.Add(decor, newPos);
        }
    }

    public void RemoveDecoration(Decoration decor) {
        if (PlacedDecorations.ContainsKey(decor)) {
            PlacedDecorations.Remove(decor);
        } else {
            throw new KeyNotFoundException("Couldnt remove decore because it isn't placed");
        }
    }
}