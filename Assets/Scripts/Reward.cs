public class Reward {
    public int Amount;
    public string Data;
    public RewardType Type;

    public static Reward Empty => new Reward() {
        Type = RewardType.None
    };
}

public enum RewardType {
    None = 0,
    Coin = 1,
    Pixel = 2,
    Shablon = 3
}