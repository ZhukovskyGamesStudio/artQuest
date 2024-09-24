using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShablonsTable : InitableManager {
    public static ShablonsTable Instance;

    [SerializeField]
    private List<ShablonConfig> _shablonConfigs;

    public static List<ShablonConfig> Shablons => Instance._shablonConfigs;
    public static ShablonConfig ShablonByName(string name) => Shablons.First(c => c.Name == name);

    public override IEnumerator Init() {
        Instance = this;
        for (int index = 0; index < _shablonConfigs.Count; index++) {
            ShablonConfig VARIABLE = _shablonConfigs[index];
            VARIABLE.Id = index;
        }

        return base.Init();
    }
}