using TMPro;
using UnityEngine;

namespace DefaultNamespace {
    public class AdminWaveConfigLineView : MonoBehaviour {
        [SerializeField]
        private TMP_Text _numberText;

        [SerializeField]
        private TMP_InputField _rewardPointsInput;

        [SerializeField]
        private TMP_InputField _difficultyPointsInput;

        private WaveData _data;

        public void Set(WaveData data, int i) {
            _data = data;
            _numberText.text = i.ToString();
            _rewardPointsInput.SetTextWithoutNotify(data.Points.ToString());
            _difficultyPointsInput.SetTextWithoutNotify(data.DifficultyPoints.ToString());
        }

        public void SetPoints(string txt) {
            _data.Points = int.Parse(txt);
        }

        public void SetDifficulty(string txt) {
            _data.DifficultyPoints = int.Parse(txt);
        }

        public WaveData Get() => _data;
    }
}