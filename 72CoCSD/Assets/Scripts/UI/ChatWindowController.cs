using System;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ChatWindowController : WindowController
    {
        public Text ChatText;
        public InputField Input;

        public IContact Contact;

        protected override void OnOpen(object context)
        {
            Input.onValidateInput += OnValidateInput;
            ChatText.text = "";
            Contact = (Customer)context;
            WriteNextLine();
        }

        private char OnValidateInput(string text, int charIndex, char addedChar)
        {
            var forced = Contact.NextForcedPlayerInput;

            if (string.IsNullOrEmpty(forced))
                return addedChar;

            return forced.Length > charIndex ? forced[charIndex] : ' ';
        }

        private void WriteNextLine()
        {
            var nextLine = Contact.Speak();

            if (string.IsNullOrEmpty(nextLine))
            {
                WriteLine(GetTimeString(), "blue", "System", 
                    string.Format("<color=blue>{0} left the chat</color>", Contact.Name));
                Input.enabled = false;
                return;
            }
            
            WriteLine(GetTimeString(), "red", Contact.Name, nextLine);
        }

        public void Send()
        {
            if (string.IsNullOrEmpty(Input.text.Trim()))
            {
                return;
            }

            string time = GetTimeString();
            WriteLine(time, "green","Player", Input.text);

            if(Contact.Read(Input.text) >= .9f)
            {
                WriteNextLine();
            }
            else
            {
                WriteLine(time, "red", Contact.Name, Contact.GetLastSentence().ToUpper());
            }

            Input.text = "";
            Input.ActivateInputField();
        }

        private string GetTimeString()
        {
            var dayTime = GameManager.Instance.Game.DayTime;
            string time = string.Format("{0:00}:{1:00} {2}",
                    dayTime.Hours < 12 ? dayTime.Hours : dayTime.Hours - 12,
                    dayTime.Minutes,
                    dayTime.Hours <= 12 ? "AM" : "PM");
            return time;
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

            var lineCount = ChatText.text.Count(c => c == '\n');
            if (lineCount >= 16)
            {
                ChatText.text = ChatText.text.Substring(ChatText.text.IndexOf('\n') + 1);
            }
        }
    }
}