using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SoundController : MonoBehaviourSingleton<SoundController>
    {
        public AudioClip StartWinAudioClip;
        public AudioClip NewMessage1AudioClip;
        public AudioClip NewMessage2AudioClip;
        public AudioClip NewContact1AudioClip;
        public AudioClip NewContact2AudioClip;

        public AudioSource ControlledAudioSource;
        
        public void PlaySound(AudioClip sound)
        {
            //var slider = GameObject.Find("SoundEffectSlider");
            var volume = PrototypeManager.Instance.GameSettings.SoundVolume;
            //if (slider != null)
            //{
            //    volume = slider.GetComponent<Slider>().value;
            //}

            var audioSource = ControlledAudioSource;
            if (audioSource != null)
            {
                audioSource.volume = volume;
                audioSource.loop = false;
                audioSource.Stop();
                audioSource.clip = sound;
                audioSource.Play();
            }
        }
    }
}