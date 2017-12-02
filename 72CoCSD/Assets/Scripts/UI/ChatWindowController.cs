using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(WindowController))]
    public class ChatWindowController : MonoBehaviour
    {
        public Text ChatText;
        public InputField Input;

        public void Send()
        {
            if (string.IsNullOrEmpty(Input.text.Trim()))
            {
                return;
            }

            ChatText.text +=
                string.Format("<color=red>11:31:00</color> <color=green>Player:</color> {0}" + Environment.NewLine,
                    Input.text);
            Input.text = "";
        }
    }
}