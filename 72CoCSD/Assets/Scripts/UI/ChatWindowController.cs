using System;
using Assets.Scripts.Managers;
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
;
            var dayTime = GameManager.Instance.Game.DayTime;
            string time = string.Format("{0:00}:{1:00} {2}",
                    dayTime.Hours < 12 ? dayTime.Hours : dayTime.Hours - 12,
                    dayTime.Minutes,
                    dayTime.Hours <= 12 ? "AM" : "PM");
            WriteLine(time, "green","Player", Input.text);
            Input.text = "";
            WriteLine(time, "red", "Customer", GameManager.Instance.Game.Issues[UnityEngine.Random.Range(0,99)].Question.ToString());
            Input.ActivateInputField();
        }

        public void WriteLine(string time,string userColor, string user, string text)
        {
            ChatText.text +=
                string.Format(
                    "<color=red>{0}</color> <color={1}>{2}</color>: {3}" + Environment.NewLine,
                    time,
                    userColor,
                    user,
                    text);
        }
    }
}