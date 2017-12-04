using System;
using Assets.Scripts.Managers;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ControlPanelWindow : WindowController
    {
        public Slider SoundVolumeSlide;
        public Text SoundVolumeText;

        public Slider SpeedSlide;
        public Text SpeedText;

        public Slider DeviationSlide;
        public Text DeviationText;

        public Slider SatisLossSlide;
        public Text SatisLossText;

        public Slider SatisGainOkSlide;
        public Text SatisGainOkText;

        public Slider SatisGainNokSlide;
        public Text SatisGainNokText;
        
        protected override void OnOpen()
        {
            var gs = PrototypeManager.Instance.GameSettings;
            SetPropertyValue(SoundVolumeSlide, SoundVolumeText, gs.SoundVolume, 0f, 1f, (v)=> gs.SoundVolume = v);
            SetPropertyValue(SpeedSlide, SpeedText, gs.MinutesPerGameTime, 1f, 6, (v) => gs.MinutesPerGameTime = v);
            SetPropertyValue(DeviationSlide, DeviationText, gs.AnswerDeviationTolerance, 0.4f, 1f, (v) => gs.AnswerDeviationTolerance = v);
            SetPropertyValue(SatisLossSlide, SatisLossText, gs.SatisfactionLossPerGameTime, 0f, 2f, (v) => gs.SatisfactionLossPerGameTime = v);
            SetPropertyValue(SatisGainOkSlide, SatisGainOkText, gs.SatisfactionGainPerCorrectAnswer, 0f, 40f, (v) => gs.SatisfactionGainPerCorrectAnswer = v);
            SetPropertyValue(SatisGainNokSlide, SatisGainNokText, gs.SatisfactionGainPerIncorrectAnswer, -5f, 5f, (v) => gs.SatisfactionGainPerIncorrectAnswer = v);
        }

        private void SetPropertyValue(Slider slider, Text text, float current, float min, float max, Action<float> onvchanged)
        {
            slider.minValue = min;
            slider.maxValue = max;
            slider.value = current;
            text.text = string.Format("{0}/{1}", Math.Round(current,2), max);
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(
                (v) =>
                {
                    text.text = string.Format("{0}/{1}", Math.Round(v,2), max);
                    onvchanged(v);
                });
        }
    }
}