using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class TavernDecorateDialog : DialogBase {
    [SerializeField]
    private Transform _gridTransform;

    [SerializeField]
    private DecorationGridView _decorationViewPrefab;

    private Dictionary<Decoration, DecorationGridView> _gridViewMap;

    private Transform _placedDecorationsHolder;
    public override DialogType Type() => DialogType.TavernDecorateDialog;
    public override Type DataType() => typeof(DecorateTavernDialogData);

    public override void Init(DialogData data) {
        base.Init(data);
        DecorateTavernDialogData decorData = data as DecorateTavernDialogData;
        _placedDecorationsHolder = decorData.PlacedDecorationsHolder;
        FillGrid();
        ShowDecorsUI();
        TavernCore.Instance.SetUiVisibility(false);
    }

    private void FillGrid() {
        _gridViewMap = new Dictionary<Decoration, DecorationGridView>();
        List<PlacedDecoration> placedList = FindObjectsOfType<PlacedDecoration>().ToList();
        foreach (PlacedDecoration placedDecoration in placedList) {
            placedDecoration.Subscribe(OnEndDrag, OnRemove);
            placedDecoration.ChangeCanDrag(true);
        }

        for (int index = 0; index < Inventory.Instance.Decorations.Count; index++) {
            Decoration decor = Inventory.Instance.Decorations[index];
            DecorationGridView view = Instantiate(_decorationViewPrefab, _gridTransform);
            view.SetData(_placedDecorationsHolder, decor, OnEndDrag, OnRemove);
            _gridViewMap.Add(decor, view);

            bool isAlreadyPlaced = Tavern.Instance.PlacedDecorations.ContainsKey(decor);
            view.SetPlaced(isAlreadyPlaced);
        }
    }

    private void OnEndDrag(Decoration decor, Vector3 pos) {
        _gridViewMap[decor].SetPlaced(true);
        Tavern.Instance.MoveDecoration(decor, pos);
    }

    private void OnRemove(Decoration decor) {
        _gridViewMap[decor].SetPlaced(false);
        Tavern.Instance.RemoveDecoration(decor);
    }

    private void ShowDecorsUI() {
        PlacedDecoration[] objs = FindObjectsOfType<PlacedDecoration>();
        foreach (var VARIABLE in objs) {
            VARIABLE.ShowRemoveButton(true);
        }
    }

    private void HideDecorsUI() {
        PlacedDecoration[] objs = FindObjectsOfType<PlacedDecoration>();
        foreach (var VARIABLE in objs) {
            VARIABLE.ShowRemoveButton(false);
        }
    }

    public override void Close() {
        HideDecorsUI();
        TavernCore.Instance.SetUiVisibility(true);
        List<PlacedDecoration> placedList = FindObjectsOfType<PlacedDecoration>().ToList();
        foreach (PlacedDecoration placedDecoration in placedList) {
            placedDecoration.Unsubscribe();
            placedDecoration.ChangeCanDrag(false);
        }

        base.Close();
    }
}

public class DecorateTavernDialogData : DialogData {
    public Transform PlacedDecorationsHolder;
    public override DialogType Type() => DialogType.TavernDecorateDialog;
}