using System.Collections.Generic;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TavernCore : MonoBehaviour {
    public static TavernCore Instance;

    [SerializeField]
    private PartyPanel _partyPanel;

    [SerializeField]
    private Transform _placedDecorationsHolder;

    [SerializeField]
    private PlacedDecoration _placedDecorationPrefab;

    [SerializeField]
    private Transform _hireButtonsHolder;

    [SerializeField]
    private GameObject _hireButtonPrefab, _hireButtonPrefab2;

    [SerializeField]
    private List<GameObject> _uiObjects;

    private List<PlacedDecoration> _placedDecorations = new List<PlacedDecoration>();

    private void Start() {
        Instance = this;
        _partyPanel.Init(StartFiring);
        PlaceDecorations();
        PlaceAdventurers();
    }

    private void PlaceDecorations() {
        _placedDecorations = new List<PlacedDecoration>();
        foreach (KeyValuePair<Decoration, Vector3> kvp in Tavern.Instance.PlacedDecorations) {
            PlacedDecoration placed = Instantiate(_placedDecorationPrefab, _placedDecorationsHolder);
            placed.transform.localPosition = kvp.Value;
            placed.Init(kvp.Key);
            _placedDecorations.Add(placed);
        }
    }

    private void PlaceAdventurers() {
        GameObject button = Instantiate(_hireButtonPrefab, _hireButtonPrefab.transform.position, quaternion.identity, _hireButtonsHolder);
        button.GetComponent<Button>().onClick.AddListener(delegate { StartHiring(FighterClassType.Nobody, button); });
        button.SetActive(true);

        foreach (var VARIABLE in _placedDecorations) {
            switch (VARIABLE.Decoration.Type) {
                case DecorationType.Chair:
                case DecorationType.Table:
                    float rnd = Random.Range(0, 1f);
                    if (rnd <= VARIABLE.Decoration.PowerPerPixel) {
                        int rndClass = Random.Range(1, 3);
                        FighterClassType t = (FighterClassType)rndClass;
                        GameObject hireButton = rndClass == 1 ? _hireButtonPrefab : _hireButtonPrefab2;

                        Vector3 finPos = VARIABLE.transform.position;
                        GameObject button2 = Instantiate(hireButton, finPos, quaternion.identity, _hireButtonsHolder);
                        button2.GetComponent<Button>().onClick.AddListener(delegate { StartHiring(t, button2); });
                        button2.SetActive(true);
                    }

                    break;
            }
        }
    }

    public void ShowTavernMenu() {
        TavernMenuDialogData data = new TavernMenuDialogData() {
            PlacedDecorationsHolder = _placedDecorationsHolder
        };
        DialogManager.Instance.ShowDialog(data);
    }

    public void StartHiring(FighterClassType type, GameObject button) {
        ClassConfig cnfg = ClassesTable.Instance.GetConfigByType(type);
        HireDialogData data = new HireDialogData() {
            ClassType = type,
            Name = "Mike",
            Cost = cnfg.Cost,
            Stats = cnfg.DefaultStats,
            OnHired = delegate(PartyMember member) {
                OnAddedMember(member);
                button.gameObject.SetActive(false);
            }
        };
        DialogManager.Instance.ShowDialog(data);
    }

    public void StartFiring(int memberIndex) {
        PartyMember member = MetaCore.Instance.Party.members[memberIndex];
        FireDialogData fireData = new FireDialogData() {
            ClassType = member.ClassType,
            MemberIndex = memberIndex,
            Name = member.Name,
            OnFired = OnRemovedMember
        };
        DialogManager.Instance.ShowDialog(fireData);
    }

    private void OnAddedMember(PartyMember member) {
        int index = MetaCore.Instance.Party.FirstEmptyPlace();
        _partyPanel.AddMember(index, member);
    }

    private void OnRemovedMember(int index) {
        _partyPanel.RemoveMemberFromPanel(index);
    }

    public void SetUiVisibility(bool isOn) {
        foreach (var VARIABLE in _uiObjects) {
            VARIABLE.SetActive(isOn);
        }
    }
}