using System;
using System.Collections;
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
        public Text TitleText;
        public Text ChatText;
        public InputField Input;

        public IContact Contact;

        protected override void OnOpen(object context)
        {
            Input.onValidateInput += OnValidateInput;
            ChatText.text = "";
            Contact = (IContact)context;
            Contact.ChatWindow = this;
            WriteNextLine(0);
        }

        private char OnValidateInput(string text, int charIndex, char addedChar)
        {
            var forced = Contact.NextForcedPlayerInput;

            if (GameManager.Instance.Game.Paused && 
                Contact is Customer)
            {
                forced =
                    "You can't speak to customer while the game is pause, stop speaking to collegues and bots to unpause the game";
            }
            

            if (string.IsNullOrEmpty(forced))
                return addedChar;

            return forced.Length > charIndex ? forced[charIndex] : ' ';
        }

        private void WriteNextLine(int delayInSeconds)
        {
            var nextLine = Contact.Speak();

            if (nextLine == null)
            {
                Contact.QuitSatified();
                Input.enabled = false;
                return;
            }

            StartCoroutine(
                WriteLineIn("red", nextLine.Author, nextLine.Text, delayInSeconds));
        }

        public void Send()
        {
            if (string.IsNullOrEmpty(Input.text.Trim()))
            {
                return;
            }

            string time = GetTimeString();
            string playerText = Input.text.Trim();
            WriteLine(time, "green","Player", playerText);

            if(Contact != null && 
               Contact.Read(playerText) >= PrototypeManager.Instance.GameSettings.AnswerDeviationTolerance)
            {
                WriteNextLine(2);
            }
            else
            {
                var last = Contact.GetLastSentence();
                StartCoroutine(
                    WriteLineIn(
                        "red",
                        last.Author,
                        last.Text.ToUpper(),
                        2));
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

        public IEnumerator WriteLineIn(string userColor, string user, string text, int delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);
            WriteLine(GetTimeString(), userColor, user, text);
        }

        public void WriteLine(string time, string userColor, string user, string text)
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

        public override void FocusWindowAndInput()
        {
            base.FocusWindowAndInput();
            Input.Select();
        }

        public void Update()
        {
            if (Contact == null)
            {
                return;
            }

            var customer = Contact as Customer;
            if (customer != null)
            {
                TitleText.text = Contact.Name + " (" + Mathf.Round(customer.Satisfaction) + ")";
                return;
            }

            TitleText.text = Contact.Name;
        }
    }
}