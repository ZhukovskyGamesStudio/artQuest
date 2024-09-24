using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {
    public static Action<int> OnNewMaxLevelReached;
    public int Coins = 0;
    public Dictionary<Color32, int> Colors = new Dictionary<Color32, int>();
    public List<Decoration> Decorations = new List<Decoration>();
    public List<Gear> Gears = new List<Gear>();
    public int MaxLevelCompleted = 0;
    public List<bool> Shablons = new List<bool>() { false, false, false, false, false };
    public static Inventory Instance => MetaCore.Instance.Inventory;

    public void InitEmpty() {
        Shablons = new List<bool>();
        Decorations = new List<Decoration>();
        foreach (ShablonConfig shablonConfig in ShablonsTable.Shablons) {
            Shablons.Add(false);
        }

        //Shablons[0] = true;
    }

    public void AddRewards(List<Reward> rewards) {
        foreach (Reward reward in rewards) {
            switch (reward.Type) {
                case RewardType.Coin:
                    AddCoin(reward.Amount);
                    break;
                case RewardType.Pixel:
                    AddPixel(ColorsTable.ColorByName(reward.Data), reward.Amount);
                    break;
                case RewardType.Shablon:
                    AddShablon(int.Parse(reward.Data));
                    break;
            }
        }
    }

    public void AddGear(Gear gear) {
        Gears.Add(gear);
    }

    public void AddDecoration(Decoration decoration) {
        Decorations.Add(decoration);
    }

    public void AddShablon(int shablonIndex) {
        Shablons[shablonIndex] = true;
    }

    public void AddCoin(int amount) {
        Coins += amount;
    }

    public void AddPixel(Color32 color, int amount) {
        if (Colors.ContainsKey(color)) {
            Colors[color] += amount;
        } else {
            Colors.Add(color, amount);
        }

        if (Colors[color] == 0) {
            Colors.Remove(color);
        } else if (Colors[color] < 0) {
            Debug.LogError("Colors less than zero");
            Colors[color] = 0;
            Colors.Remove(color);
        }
    }

    public void TrySetMaxLevelCompleted(int completed) {
        if (completed <= MaxLevelCompleted) {
            return;
        }

        MaxLevelCompleted = completed;
        OnNewMaxLevelReached?.Invoke(completed);
    }
}