using System;
using System.Collections.Generic;
using DefaultNamespace;
using FreeDraw.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DrawingDialog : DialogBase {
    [SerializeField]
    private TMP_Text _coinsAmount;

    [SerializeField]
    private ShablonsPanel _shablonsPanel;

    [SerializeField]
    private GameObject _drawingPanel;

    [SerializeField]
    private CreateGearPanel _createGearPanel;

    [SerializeField]
    private string filePath = "C:\\Users\\grafe\\Desktop\\";

    [SerializeField]
    private TMP_InputField _nameInput;

    [SerializeField]
    private Image _drawableImage;

    private ShablonConfig _currentShablon;
    private Dictionary<Color32, int> _drawnPixels;
    private int _drawnPixelsAmount;

    private int shablonIndex;

    private Drawable Drawable => DrawableAdapter.Drawable;

    private void Start() {
        Drawable.OnPixelDrawn += OnPixelDrawn;
        Drawable.OnPixelErased += OnPixelErased;
        Init();
    }

    private void Update() {
        _drawableImage.sprite = DrawableAdapter.GetCameraView;
    }

    public void Init() {
        _shablonsPanel.InitShablonsGrid();
        OpenShablonsAndResetCanvas();
        UpdateCoinsText();
    }

    public void OpenShablonsButton() {
        Drawable.ClearAll();
        OpenShablonsAndResetCanvas();
    }

    private void OpenShablonsAndResetCanvas() {
        DrawableAdapter.SetDrawableActive(false);
        _shablonsPanel.gameObject.SetActive(true);
        _drawingPanel.gameObject.SetActive(false);
        _drawnPixels = new Dictionary<Color32, int>();
        _drawnPixelsAmount = 0;
        foreach (ColorsSet var in FindObjectsByType<ColorsSet>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)) {
            var.ResetPixelAmounts();
        }

        _createGearPanel.gameObject.SetActive(false);
    }

    public void SelectShablon(ShablonConfig config) {
        _currentShablon = config;
        DrawableAdapter.SetShablonView(_currentShablon.Sprite);
        StartNewDraw();
        _createGearPanel.SetShablon(config, MetaCore.Instance.Inventory.Coins);
    }

    public void StartNewDraw() {
        DrawableAdapter.SetDrawableActive(true);
        Sprite sprite = SpriteCloner.CloneSprite(_currentShablon.Sprite);
        _nameInput.SetTextWithoutNotify(_currentShablon.Name + Random.Range(0, 100));
        Drawable.SetSprite(sprite);
        Drawable.SetShablon(_currentShablon.Sprite);
        Drawable.ResetCanvas();
        _createGearPanel.gameObject.SetActive(true);
        _drawingPanel.gameObject.SetActive(true);
    }

    private void OnPixelDrawn(Color32 color) {
        if (_drawnPixels.ContainsKey(color)) {
            _drawnPixels[color]++;
        } else {
            _drawnPixels.Add(color, 1);
        }

        _drawnPixelsAmount++;
        _createGearPanel.UpdateData(_drawnPixels);
    }

    private void OnPixelErased(Color32 color) {
        if (_drawnPixels.ContainsKey(color)) {
            _drawnPixels[color]--;
            if (_drawnPixels[color] == 0) {
                _drawnPixels.Remove(color);
            }

            _drawnPixelsAmount--;
            _createGearPanel.UpdateData(_drawnPixels);
        }
    }

    public void SaveResult() {
        Sprite sprite = Drawable.GetSprite();
        sprite.name = _nameInput.text;
        sprite.texture.name = _nameInput.text;
        FighterStats stats = _createGearPanel.GetStats();
        int cost = _createGearPanel.GetCost();

        if (_currentShablon is GearShablonConfig gearShablon) {
            Gear finGear = new Gear() {
                Type = gearShablon.GearType,
                Sprite = sprite,
                Name = _nameInput.text,
                Stats = stats
            };
            MetaCore.Instance.Inventory.AddGear(finGear);
        } else if (_currentShablon is DecorationShablonConfig decorationShablon) {
            Decoration finDecoration = new Decoration() {
                Sprite = sprite,
                Name = _nameInput.text,
                Type = decorationShablon.DecorationType,
                PowerPerPixel = decorationShablon.PowerPerPixel * _drawnPixelsAmount
            };
            MetaCore.Instance.Inventory.AddDecoration(finDecoration);
        }

        MetaCore.Instance.Inventory.AddCoin(-cost);
        UpdateCoinsText();
        SaveUsedPixels();
        OpenShablonsAndResetCanvas();
        DrawableAdapter.SetDrawableActive(false);
        //SaveToFile(sprite);
    }

    private void UpdateCoinsText() {
        _coinsAmount.text = $"You have {MetaCore.Instance.Inventory.Coins}";
    }

    private void SaveUsedPixels() {
        foreach (var kvp in _drawnPixels) {
            MetaCore.Instance.Inventory.AddPixel(kvp.Key, -kvp.Value);
        }
    }

    private void SaveToFile(Sprite sprite) {
        string fullNamePath = filePath + _nameInput.text;
        PngSaver.SaveSprite(sprite, fullNamePath);
    }

    public void SetPen() {
        DrawableAdapter.Tools.SetPen();
    }

    public void SetBrush() {
        DrawableAdapter.Tools.SetBrush();
    }

    public void SetEraser() {
        DrawableAdapter.Tools.SetEraser();
    }

    public void SetClearAll() {
        DrawableAdapter.Tools.ClearAllPixels();
    }

/*
    public void AlterSprite()
    {
// Create a copy of the texture by reading and applying the raw texture data.
        Texture2D newTexture = CloneTexture(defaultSprite.texture);

// Alter the cloned texture
// ---------------------------------------------------------
        var newData = newTexture.GetRawTextureData<Color32>();

// Overwrite sections
        for (int x = 0; x < 32; x++)
        {
            var index = x;
            newData[index] = new Color(0, 0, 0, 1.0f);
        }

// Finalise the new texture
        newTexture.LoadRawTextureData<Color32>(newData);
        newTexture.Apply();

// Final product
        currentSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f), defaultSprite.pixelsPerUnit);

// Send to the sprite render
        spriteRenderer.sprite = currentSprite;
    }*/
    public override DialogType Type() => DialogType.DrawingDialog;

    public override Type DataType() => typeof(DrawingDialogData);
}

public class DrawingDialogData : DialogData {
    public override DialogType Type() => DialogType.DrawingDialog;
}