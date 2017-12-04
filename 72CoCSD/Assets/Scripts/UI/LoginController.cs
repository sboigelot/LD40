using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class LoginController : MonoBehaviour
    {
        public InputField PasswordInput;
        public GameObject LoginPanel;

        public void ArrowClick()
        {
            if (PasswordInput.text.ToLower() == "password")
            {
                LoginPanel.SetActive(false);
                PasswordInput.text = "";

                SoundController.Instance.PlaySound(SoundController.Instance.StartWinAudioClip);
            }
        }
    }
}