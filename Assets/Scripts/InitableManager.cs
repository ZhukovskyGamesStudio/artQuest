using System.Collections;
using UnityEngine;

public class InitableManager : MonoBehaviour {
    public virtual IEnumerator Init() {
        yield break;
    }
}