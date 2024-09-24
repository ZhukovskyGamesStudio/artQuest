using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fighter : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer _spriteRenderer, _weaponSpriteRenderer, _helmetSpriteRenderer;

    [SerializeField]
    private TMP_Text _statsText;

    [SerializeField]
    private GameObject _selectedIcon;

    [SerializeField]
    private Sprite _transparentSprite;

    [SerializeField]
    private Animation _animation;

    [SerializeField]
    private FighterClassType _classType;

    public bool IsAlly;
    private FighterStats _defaultStats = new FighterStats();
    private Action<Fighter> _onMouseClick;
    private Gear _weapon, _helmet;
    public FighterStats Stats { get; private set; } = new FighterStats();

    public Sprite GetClassIcon => ClassesTable.Instance.GetConfigByType(_classType).Icon64;

    private void OnMouseUpAsButton() {
        _onMouseClick?.Invoke(this);
    }

    public void SetDefaultStats(FighterStats stats) {
        _defaultStats = stats;
        UpdateStats(stats);
    }

    public void SetSprite(Sprite sprite) {
        _spriteRenderer.sprite = sprite;
    }

    public FighterStats GetFullStats() {
        FighterStats stats = new FighterStats();
        stats.Add(_defaultStats);
        if (_weapon != null) {
            stats.Add(_weapon.Stats);
        }

        if (_helmet != null) {
            stats.Add(_helmet.Stats);
        }

        return stats;
    }

    public void SetGear(Gear gear) {
        if (gear.Type == GearType.Helmet) {
            _helmet = gear;
            _helmetSpriteRenderer.sprite = gear.Sprite;
        }

        if (gear.Type == GearType.Weapon) {
            _weapon = gear;
            _weaponSpriteRenderer.sprite = gear.Sprite;
        }

        UpdateStats(GetFullStats());
    }

    public void RemoveGear(GearType type) {
        if (type == GearType.Helmet) {
            _helmet = null;
            _helmetSpriteRenderer.sprite = _transparentSprite;
        }

        if (type == GearType.Weapon) {
            _weapon = null;
            _weaponSpriteRenderer.sprite = _transparentSprite;
        }

        UpdateStats(GetFullStats());
    }

    public IEnumerator TryTakeDamage(int damage, int cleanDamage) {
        float dodgeRoll = Random.Range(0, 1f);
        if (dodgeRoll <= Stats.DodgeChance) {
            yield return StartCoroutine(DodgeAnimation());
            yield break;
        }

        int calculatedDamage = Mathf.Max(1, damage - Stats.Defense) + cleanDamage;
        Stats.Hp -= calculatedDamage;
        UpdateStats(Stats);
        if (Stats.Hp <= 0) {
            yield return StartCoroutine(DeathAnimation());
        } else {
            yield return StartCoroutine(TakeDamageAnimation());
        }
    }

    public void UpdateStats(FighterStats stats) {
        Stats = stats;
        if (stats.Hp <= 0) {
            _statsText.text = "";
            return;
        }

        _statsText.text = stats.GetString();
    }

    public IEnumerator AttackAnimation() {
        if (_animation != null && _animation.GetClip("Attack") != null) {
            _animation.Play("Attack");
            yield return new WaitWhile(() => _animation.isPlaying);
        } else {
            yield return new WaitForSeconds(0.1f);
            transform.localScale = transform.localScale * 1.1f;
            yield return new WaitForSeconds(0.1f);
            transform.localScale = transform.localScale * 0.9f;
            yield return new WaitForSeconds(0.1f);
            transform.localScale = Vector3.one;
        }
    }

    public IEnumerator TakeDamageAnimation() {
        yield return new WaitForSeconds(0.05f);
        transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(0.05f);
        transform.localScale = Vector3.one;
        yield return new WaitForSeconds(0.05f);
        transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(0.05f);
        transform.localScale = Vector3.one;
    }

    public IEnumerator DeathAnimation() {
        if (_animation != null && _animation.GetClip("Death") != null) {
            _animation.Play("Death");
            yield return new WaitWhile(() => _animation.isPlaying);
        } else {
            yield return new WaitForSeconds(0.1f);
            transform.localScale = transform.localScale * 0.5f;
            yield return new WaitForSeconds(0.1f);
            transform.localScale = transform.localScale * 0.5f;
            yield return new WaitForSeconds(0.1f);
            transform.localScale = Vector3.zero;
        }
    }

    //TODO Dodge animation
    public IEnumerator DodgeAnimation() {
        if (_animation != null && _animation.GetClip("Dodge") != null) {
            _animation.Play("Dodge");
            yield return new WaitWhile(() => _animation.isPlaying);
        } else {
            yield return new WaitForSeconds(0.1f);
            transform.localScale = transform.localScale * 1.1f;
            yield return new WaitForSeconds(0.1f);
            transform.localScale = transform.localScale * 0.9f;
            yield return new WaitForSeconds(0.1f);
            transform.localScale = Vector3.one;
        }
    }

    public void ToggleSelect(bool isOn) {
        _selectedIcon.SetActive(isOn);
    }

    public void SetClickable(Action<Fighter> onSelectAlly) {
        _onMouseClick = onSelectAlly;
    }

    public void ClearClickable() {
        _onMouseClick = null;
    }
}