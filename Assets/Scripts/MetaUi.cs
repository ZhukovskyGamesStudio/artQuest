using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaUi : MonoBehaviour {
    [SerializeField]
    private DrawingDialog _drawingDialog;

    [SerializeField]
    private Animation _cameraAnimation;

    private AsyncOperation _asyncOperation;

    private void Start() {
        StartCoroutine(PreLoadSceneAsync());
    }

    IEnumerator PreLoadSceneAsync() {
        //Begin to load the Scene you specify
        _asyncOperation = SceneManager.LoadSceneAsync("FightScene");
        //Don't let the Scene activate until you allow it to
        _asyncOperation.allowSceneActivation = false;
        yield break;
    }

    public void TryGoOnAdventure() {
        if (MetaCore.Instance.Party.IsEmpty) {
            TalkDialogData data = new TalkDialogData() {
                IconType = TalkIconType.MainCharacter,
                Text = "В моей команде нет ни одного героя. В одиночку нельзя отправляться в путешествие."
            };
            DialogManager.Instance.ShowDialog(data);
            return;
        }

        if (_asyncOperation != null) {
            _asyncOperation.allowSceneActivation = true;
        } else {
            SceneManager.LoadScene("FightScene");
        }
    }

    public void MoveToTavern() {
        _cameraAnimation.Play("MoveToTavern");

        TalkDialogData data = new TalkDialogData() {
            IconType = TalkIconType.MainCharacter,
            Text = "Загляну-ка я в таверну..."
        };
        DialogManager.Instance.ShowDialog(data);
    }

    public void MoveFromTavern() {
        _cameraAnimation.Play("MoveFromTavern");
    }

    public void MoveToDrawing() {
        if (MetaCore.Instance.Inventory.Shablons.All(s => !s)) {
            TalkDialogData talkData = new TalkDialogData() {
                IconType = TalkIconType.MainCharacter,
                Text = "Дверь в этот дом наглухо закрыта..."
            };
            DialogManager.Instance.ShowDialog(talkData);
            return;
        }

        _cameraAnimation.Play("MoveToDrawing");
        TalkDialogData data = new TalkDialogData() {
            IconType = TalkIconType.MainCharacter,
            Text = "Зайду в свою мастерскую..."
        };
        DialogManager.Instance.ShowDialog(data);
    }

    public void MoveFromDrawing() {
        _cameraAnimation.Play("MoveFromDrawing");
    }
}