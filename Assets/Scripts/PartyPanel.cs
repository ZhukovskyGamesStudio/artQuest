using System;
using System.Linq;
using UnityEngine;

public class PartyPanel : MonoBehaviour {
    [SerializeField]
    private PartyPanelMemberView[] _panelMemberViews;

    private Action<int> _onTryRemoveMember;

    public void Init(Action<int> onTryRemoveMember) {
        _onTryRemoveMember = onTryRemoveMember;
        foreach (PartyPanelMemberView memberView in _panelMemberViews) {
            memberView.Clear();
        }
    }

    public void AddMember(int index, PartyMember member) {
        _panelMemberViews[index].Set(member);
    }

    public void TryRemoveMember(int index) {
        if (_panelMemberViews.Count(m => !m.IsEmpty) == 1) {
            ErrorDialogData data = new ErrorDialogData() {
                Text = "Вы не можете выгнать последнего героя!"
            };
            DialogManager.Instance.ShowDialog(data);
            return;
        }

        _onTryRemoveMember?.Invoke(index);
    }

    public void RemoveMemberFromPanel(int index) {
        _panelMemberViews[index].Clear();
    }
}