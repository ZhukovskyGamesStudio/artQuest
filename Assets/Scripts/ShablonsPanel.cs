using System.Collections.Generic;
using UnityEngine;

public class ShablonsPanel : MonoBehaviour {
    [SerializeField]
    private DrawingDialog _drawingDialog;

    [SerializeField]
    private ShablonView _shablonViewPrefab;

    [SerializeField]
    private Transform _shablonsGrid;

    private bool _isGridCreated;
    private List<ShablonView> _shablonViews;

    List<ShablonConfig> ShablonConfigs => ShablonsTable.Shablons;

    public void InitShablonsGrid() {
        if (!_isGridCreated) {
            CreateGrid();
        }

        for (int index = 0; index < ShablonConfigs.Count; index++) {
            ShablonConfig shablonConfig = ShablonConfigs[index];
            ShablonView view = _shablonViews[index];
            int clockingIndex = index;

            view.SetData(shablonConfig.name, shablonConfig.Sprite, MetaCore.Instance.Inventory.Shablons[index],
                delegate { SelectShablon(clockingIndex); });
        }
    }

    private void CreateGrid() {
        _shablonViews = new List<ShablonView>();
        for (int index = 0; index < ShablonConfigs.Count; index++) {
            ShablonView view = Instantiate(_shablonViewPrefab, _shablonsGrid);
            _shablonViews.Add(view);
        }

        _isGridCreated = true;
    }

    public void SelectShablon(int shablonIndex) {
        _drawingDialog.SelectShablon(ShablonConfigs[shablonIndex]);
        gameObject.SetActive(false);
    }
}