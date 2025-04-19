using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FightCore : MonoBehaviour {
    [SerializeField]
    private Button _leaveButton, _nextRoundButton;

    [SerializeField]
    private WavesConfig _wavesConfig;

    [SerializeField]
    private GameObject _inFightButtons;

    [SerializeField]
    private EndFightPanel _endFightPanel;

    [SerializeField]
    private TMP_Text _roundText;

    [SerializeField]
    private Fighter _enemyPrefab;

    [SerializeField]
    private List<Transform> _allyPos, _enemyPos;

    [SerializeField]
    private GearsPanel _gearsPanel;

    [SerializeField]
    private RewardsPanel _rewardsPanel;

    [SerializeField]
    private RewardsConfig _rewardsConfig;

    [SerializeField]
    private FightBackground _fightBackground;

    private List<Reward> _accumulatedRewards = new List<Reward>();

    private List<Fighter> _allies, _enemies;

    private Queue<Transform> _allyPosQueue, _enemyPosQueue;
    private List<Reward> _halfedRewards = new List<Reward>();
    //private bool _isLost = false;
    private RewardGenerator _rewardGenerator;
    private int _wave;

    private void Awake() {
        _nextRoundButton.onClick.AddListener(NextRound);
        _leaveButton.onClick.AddListener(GoToDrawingScene);
        _rewardGenerator = new RewardGenerator(_rewardsConfig);
    }

    private void Start() {
        ClearQueues();

        _allies = new List<Fighter>();
        foreach (var VARIABLE in MetaCore.Instance.Party.members) {
            if (VARIABLE == null) {
                continue;
            }

            Fighter shablon = ClassesTable.Instance.GetConfigByType(VARIABLE.ClassType).Fighter;
            Fighter ally = Instantiate(shablon, _allyPosQueue.Dequeue());
            FighterStats allyStats = new FighterStats();
            allyStats.Add(VARIABLE.Stats);
            ally.SetDefaultStats(allyStats);
            ally.IsAlly = true;
            _allies.Add(ally);
        }

        if (MetaCore.Instance.Inventory.Gears.Count > 0) {
            OpenGearsPanel();
        }
    }

    private void ClearQueues() {
        _allyPosQueue = new Queue<Transform>(_allyPos);
        _enemyPosQueue = new Queue<Transform>(_enemyPos);
    }

    private void OpenGearsPanel() {
        _gearsPanel.gameObject.SetActive(true);
        _gearsPanel.InitGears(MetaCore.Instance.Inventory, _allies);
    }

    private void CloseGearsPanel() {
        _gearsPanel.gameObject.SetActive(false);
        _gearsPanel.OnClose();
    }

    public void StartFight() {
        CloseGearsPanel();
        _inFightButtons.gameObject.SetActive(true);
        _wave = -1;
        GenerateEnemiesForWave(0);
        UpdateWaveText(1);
    }

    public void SelectGear(Gear gear, Fighter ally) {
        ally.SetGear(gear);
    }

    public void SelectAlly(Fighter ally) {
        foreach (Fighter VARIABLE in _allies) {
            VARIABLE.ToggleSelect(VARIABLE == ally);
        }
    }

    public void GoToDrawingScene() {
        EndRound(EndRoundState.Leave);
    }

    public void NextRound() {
        _wave++;
        _inFightButtons.gameObject.SetActive(false);
        UpdateWaveText(_wave + 1);

        StartCoroutine(FightCoroutine(_allies.Concat(_enemies)));
    }

    private void GenerateEnemiesForWave(int wave) {
        if (_enemies != null && _enemies.Count > 0) {
            foreach (var fighter in _enemies) {
                Destroy(fighter.gameObject);
            }
        }

        _enemyPosQueue = new Queue<Transform>(_enemyPos);
        _enemies = new List<Fighter>();
        WaveData waveData = _wavesConfig.GetWaveDataByIndex(wave);
        List<EnemyConfig> enemyConfigs = waveData.PossibleEnemies;
        foreach (var enemyConfig in enemyConfigs) {
            Fighter enemy = Instantiate(_enemyPrefab, _enemyPosQueue.Dequeue());
            enemy.SetSprite(enemyConfig.Sprite);
            FighterStats enemyStats = new FighterStats();
            enemyStats.Add(enemyConfig.GetStatsByDifficultyPoints(waveData.DifficultyPoints));
            enemy.UpdateStats(enemyStats);
            _enemies.Add(enemy);
        }

        TryUpdateBackground(waveData);
    }

    private void TryUpdateBackground(WaveData data) {
        if (!data.IsChangingBackground) {
            return;
        }

        _fightBackground.SetData(data.BackgroundSprite, data.BackgroundName);
    }

    private IEnumerator FightCoroutine(IEnumerable<Fighter> fighters) {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 1000; i++) {
            foreach (var VARIABLE in fighters) {
                VARIABLE.Stats.MovePoints = VARIABLE.Stats.Speed;
            }

            fighters = fighters.OrderByDescending(f => f.Stats.MovePoints);

            foreach (var fighter in fighters) {
                if (fighter.Stats.Hp <= 0) {
                    continue;
                }

                FighterStats st = fighter.Stats;
                int dmg = st.Damage;
                yield return StartCoroutine(fighter.AttackAnimation());
                if (fighter.IsAlly) {
                    var aliveEnemies = fighters.Where(f => !f.IsAlly && f.Stats.Hp > 0);
                    Fighter target = aliveEnemies.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                    if (fighter.Stats.SwipeDamage > 0) {
                        int fullDamage = dmg + st.SwipeDamage;
                        foreach (Fighter enemy in aliveEnemies) {
                            int calcDamage = enemy == target ? fullDamage : st.SwipeDamage;
                            yield return StartCoroutine(enemy.TryTakeDamage(calcDamage, st.CleanDamage));
                        }
                    } else {
                        yield return StartCoroutine(target.TryTakeDamage(dmg, st.CleanDamage));
                    }
                } else {
                    var aliveAllies = fighters.Where(f => f.IsAlly && f.Stats.Hp > 0);
                    Fighter target = aliveAllies.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                    yield return StartCoroutine(target.TryTakeDamage(dmg, st.CleanDamage));
                }

                st.MovePoints = 0;

                if (fighters.Where(f => f.IsAlly).All(f => f.Stats.Hp <= 0)) {
                    EndRound(EndRoundState.Lose);
                    yield break;
                }

                if (fighters.Where(f => !f.IsAlly).All(f => f.Stats.Hp <= 0)) {
                    if (_wave == _wavesConfig._waves.Count - 1) {
                        EndRound(EndRoundState.ReachEnd);
                    } else {
                        EndRound(EndRoundState.Win);
                        yield return new WaitForSeconds(0.25f);
                        GenerateEnemiesForWave(_wave + 1);
                        _inFightButtons.gameObject.SetActive(true);
                    }

                    yield break;
                }
            }
        }
    }

    private void EndRound(EndRoundState state) {
        if (state == EndRoundState.Win) {
            AddRewardsToPanel();
            Inventory.Instance.TrySetMaxLevelCompleted(_wave);
        } else if (state == EndRoundState.ReachEnd) {
            AddRewardsToPanel();
            _endFightPanel.OpenWinState(_accumulatedRewards);
            _inFightButtons.gameObject.SetActive(false);
            Inventory.Instance.TrySetMaxLevelCompleted(_wave);
        } else if (state == EndRoundState.Leave) {
            _endFightPanel.OpenLeaveState(_accumulatedRewards);
            _inFightButtons.gameObject.SetActive(false);
        } else {
            //_isLost = true;
            _halfedRewards = CutRewardsByPercent(_accumulatedRewards, 0.5f);
            _endFightPanel.OpenLoseState(_halfedRewards, _accumulatedRewards);
            _inFightButtons.gameObject.SetActive(false);
        }
    }

    private void AddRewardsToPanel() {
        List<Reward> rewards = _rewardGenerator.GenerateRewardForWave(_wave, _wavesConfig._waves[_wave].Points);
        _accumulatedRewards = CombineRewards(_accumulatedRewards, rewards);
        _rewardsPanel.gameObject.SetActive(true);
        _rewardsPanel.SetRewardsWithAnimation(_accumulatedRewards, true);
    }

    private void UpdateWaveText(int wave) {
        _roundText.text = wave.ToString();
    }

    private List<Reward> CutRewardsByPercent(List<Reward> rewards, float percent) {
        List<Reward> halfedRewards = new List<Reward>();
        foreach (var reward in rewards) {
            int newAmount = reward.Amount / 2;
            if (newAmount > 0) {
                halfedRewards.Add(new Reward() {
                    Amount = newAmount,
                    Type = reward.Type,
                    Data = reward.Data
                });
            }
        }

        return halfedRewards;
    }

    private static List<Reward> CombineRewards(List<Reward> a, List<Reward> b) {
        foreach (var reward in b) {
            Reward same = a.FirstOrDefault(r => r.Type == reward.Type && r.Data == reward.Data);
            if (same != null && same.Type != RewardType.None) {
                same.Amount += reward.Amount;
            } else {
                a.Add(reward);
            }
        }

        return a;
    }
}

[Serializable]
public class FighterStats {
    public int Hp;
    public int Speed;
    public int Damage;
    public int MovePoints;

    [Header("Special Stats")]
    public int SwipeDamage;

    public int Defense;
    public int CleanDamage;
    public float DodgeChance;

    public void Add(FighterStats b) {
        Hp += b.Hp;
        Speed += b.Speed;
        Damage += b.Damage;
        MovePoints += b.MovePoints;
        SwipeDamage += b.SwipeDamage;
        Defense += b.Defense;
        CleanDamage += b.CleanDamage;
        DodgeChance += b.DodgeChance;

        ClampValues();
    }

    public void Multiply(float b) {
        Hp = Mathf.Max(1, Mathf.FloorToInt(Hp * b));
        Speed = Mathf.Max(1, Mathf.FloorToInt(Speed * b));
        Damage = Mathf.Max(1, Mathf.FloorToInt(Damage * b));
        MovePoints = Mathf.Max(1, Mathf.FloorToInt(MovePoints * b));
        SwipeDamage = Mathf.Max(0, Mathf.FloorToInt(SwipeDamage * b));
        Defense = Mathf.Max(0, Mathf.FloorToInt(Defense * b));
        CleanDamage = Mathf.Max(0, Mathf.FloorToInt(CleanDamage * b));
        DodgeChance *= b;

        ClampValues();
    }

    public void MultiplyRandom(float min, float max) {
        Hp = Mathf.Max(1, Mathf.FloorToInt(Hp * Random.Range(min, max)));
        Speed = Mathf.Max(1, Mathf.FloorToInt(Speed * Random.Range(min, max)));
        Damage = Mathf.Max(1, Mathf.FloorToInt(Damage * Random.Range(min, max)));
        MovePoints = Mathf.Max(1, Mathf.FloorToInt(MovePoints * Random.Range(min, max)));

        SwipeDamage = Mathf.Max(0, Mathf.FloorToInt(SwipeDamage * Random.Range(min, max)));
        Defense = Mathf.Max(0, Mathf.FloorToInt(Defense * Random.Range(min, max)));
        CleanDamage = Mathf.Max(0, Mathf.FloorToInt(CleanDamage * Random.Range(min, max)));
        DodgeChance *= Random.Range(min, max);

        ClampValues();
    }

    public string GetString() {
        string statsString = "";
        if (Hp != 0) {
            statsString += $"\nHp: {Hp}";
        }

        if (Damage != 0) {
            statsString += $"\nDamage: {Damage}";
        }

        if (Speed != 0) {
            statsString += $"\nSpeed: {Speed}";
        }

        if (SwipeDamage != 0) {
            statsString += $"\nSwipeDamage: {SwipeDamage}";
        }

        if (Defense != 0) {
            statsString += $"\nDefense: {Defense}";
        }

        if (CleanDamage != 0) {
            statsString += $"\nCleanDamage: {CleanDamage}";
        }

        if (DodgeChance != 0) {
            statsString += $"\nDodgeChance: {DodgeChance * 100}%";
        }

        return statsString;
    }

    private void ClampValues() {
        DodgeChance = Mathf.Clamp(DodgeChance, 0, 0.95f);
    }
}

public enum EndRoundState {
    Lose,
    Win,
    Leave,
    ReachEnd
}