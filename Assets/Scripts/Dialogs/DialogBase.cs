using System;
using UnityEngine;

public abstract class DialogBase : MonoBehaviour {
    public abstract DialogType Type();
    public abstract Type DataType();

    public virtual void Init(DialogData data) {
        if (data.GetType() != DataType()) {
            throw new ArgumentException("Passed wrong dialogData type: " + data.GetType());
        }
    }

    public virtual void Close() {
        Destroy(gameObject);
    }
}