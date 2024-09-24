using System;
using System.Linq;
using UnityEngine;

[SerializeField]
public class Party {
    public PartyMember[] members = new PartyMember[5];

    public bool HasEmptyPlace => members.Any(m => m == null);

    public bool IsEmpty => members.All(m => m == null);

    public int FirstEmptyPlace() {
        for (int i = 0; i < members.Length; i++) {
            if (members[i] == null) {
                return i;
            }
        }

        throw new Exception("No empty places found");
    }

    public void AddMember(PartyMember member) {
        members[FirstEmptyPlace()] = member;
    }

    public void RemoveMember(int memberIndex) {
        members[memberIndex] = null;
    }

    public void Clear() {
        members = new PartyMember[5];
    }
}