using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClassesTable : InitableManager {
    public static ClassesTable Instance;

    [SerializeField]
    private List<ClassConfig> _classesConfigs = new List<ClassConfig>();

    public ClassConfig GetConfigByType(FighterClassType type) => _classesConfigs.FirstOrDefault(c => c.Type == type);

    public override IEnumerator Init() {
        Instance = this;
        return base.Init();
    }
}