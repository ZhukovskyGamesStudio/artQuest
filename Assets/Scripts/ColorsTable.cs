using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorsTable : InitableManager {
    public static ColorsTable Instance;

    [SerializeField]
    private List<PixelConfig> _colorConfigs;

    public static List<PixelConfig> Colors => Instance._colorConfigs;
    public static Color ColorByName(string name) => Colors.First(c => c.Name == name).Color;
    public static string NameByColor(Color color) => Colors.First(c => c.Color == color).Name;
    public static PixelConfig ConfigByColor(Color color) => Colors.First(c => c.Color == color);

    public override IEnumerator Init() {
        Instance = this;
        return base.Init();
    }
}