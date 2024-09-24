using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GearsPanel : MonoBehaviour {
    [SerializeField]
    private float _zoomedCameraSize = 2.5f;

    [SerializeField]
    private FightCore _fightCore;

    [SerializeField]
    private GearView _gearViewPrefab;

    [SerializeField]
    private Transform _gearsGrid;

    [SerializeField]
    private GameObject _deselectAllyButton, _readyButton;

    private List<Fighter> _allies = new List<Fighter>();

    private Fighter _ally;
    private Vector3 _defaultCameraPos;
    private float _defaultCameraSize;

    private List<Gear> _gears;
    private Dictionary<GearView, Fighter> gearsEquipped = new Dictionary<GearView, Fighter>();

    private void Awake() {
        _defaultCameraSize = Camera.main.orthographicSize;
        _defaultCameraPos = new Vector3(0, 0, -10);
        _deselectAllyButton.gameObject.SetActive(false);
    }

    public void InitGears(Inventory inventory, List<Fighter> allies) {
        _allies = allies;
        _gears = new List<Gear>(inventory.Gears);
        _gears.Reverse();
        for (int index = 0; index < _gears.Count; index++) {
            Gear VARIABLE = _gears[index];
            GearView view = Instantiate(_gearViewPrefab, _gearsGrid);
            int tmp = index;
            view.SetData(VARIABLE.Name, VARIABLE.Sprite, VARIABLE.Type, delegate(GearView gearView) { SelectGear(gearView, tmp); });
            gearsEquipped.Add(view, null);
        }

        InitAllyButtons();
    }

    private void InitAllyButtons() {
        foreach (Fighter ally in _allies) {
            ally.SetClickable(SelectAlly);
        }
    }

    private void ClearAllyButtons() {
        foreach (Fighter ally in _allies) {
            ally.ClearClickable();
        }
    }

    private void ChangeCameraZoom(float newVal, Vector3 targetPos) {
        Camera.main.orthographicSize = newVal;
        Camera.main.transform.position = targetPos;
    }

    public void OnClose() {
        ChangeCameraZoom(_defaultCameraSize, _defaultCameraPos);
        ClearAllyButtons();
    }

    //TODO remake via DRAG'N'DRAP
    private void SelectGear(GearView gearView, int gear) {
        if (_ally == null) {
            return;
        }

        if (gearsEquipped[gearView] == _ally) {
            DeselectGear(gearView);
            return;
        }

        KeyValuePair<GearView, Fighter> sameTypeGear =
            gearsEquipped.FirstOrDefault(ge => ge.Key.GearType == gearView.GearType && ge.Value == _ally);
        if (sameTypeGear.Key != null) {
            DeselectGear(sameTypeGear.Key);
        }

        gearView.CloseLock(_ally);
        _fightCore.SelectGear(_gears[gear], _ally);

        gearsEquipped[gearView] = _ally;
    }

    private void DeselectGear(GearView gearView) {
        if (_ally == null) {
            return;
        }

        gearView.OpenLock();
        _ally.RemoveGear(gearView.GearType);
        gearsEquipped[gearView] = null;
    }

    private void SelectAlly(Fighter ally) {
        _ally = ally;
        _fightCore.SelectAlly(ally);
        Vector3 newPos = _ally.transform.position;
        newPos.z = -10;

        ChangeCameraZoom(_zoomedCameraSize, newPos);
        _deselectAllyButton.gameObject.SetActive(true);
        _readyButton.gameObject.SetActive(false);
        ResortGrid();
    }

    private void ResortGrid() {
        int i = 0;
        foreach (KeyValuePair<GearView, Fighter> item in gearsEquipped.OrderByDescending(key => key.Value == _ally)) {
            item.Key.transform.SetSiblingIndex(i);
            bool isOnOtherAlly = item.Value != _ally && item.Value != null;
            item.Key.gameObject.SetActive(!isOnOtherAlly);
            i++;
        }
    }

    public void DeselectAlly() {
        _ally = null;
        _fightCore.SelectAlly(_ally);
        ChangeCameraZoom(_defaultCameraSize, _defaultCameraPos);
        _deselectAllyButton.gameObject.SetActive(false);
        _readyButton.gameObject.SetActive(true);
        ResortGrid();
    }
}