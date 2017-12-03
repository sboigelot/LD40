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

        public CustomerProgressController CustomerProgress;
        public GameObject CustomerProgressPanel;

        public int PlayerMistakeCount;
        public IContact Contact;

        protected override void OnOpen(object context)
        {
            Input.onValidateInput += OnValidateInput;
            ChatText.text = "";
            Contact = (IContact)context;
            CustomerProgressPanel.SetActive(Contact is Customer);
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
            PlayerMistakeCount = 0;
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
            if (string.IsNullOrEmpty(Input.text.Trim()) || Contact == null)
            {
                return;
            }

            string time = GetTimeString();
            string playerText = Input.text.Trim();
            var effectivenes = Contact.Read(playerText);
            var understood = effectivenes >= PrototypeManager.Instance.GameSettings.AnswerDeviationTolerance;

            string text = string.Format("{0}\t\t<i>(<color={1}>{2}</color> {3}% correct)</i>",
                playerText,
                understood ? "green" : "red",
                understood ? "V" : "X",
                Math.Round(effectivenes, 2) * 100);

            WriteLine(time, "green","Player", text);

            if(understood)
            {
                WriteNextLine(1);
            }
            else
            {
                PlayerMistakeCount++;
                if (PlayerMistakeCount > 4)
                {
                    ((Customer)Contact).RageQuit();
                }
                else
                {
                    var last = Contact.GetLastSentence();
                    var lastText = last.Text;
                    if (PlayerMistakeCount > 1)
                    {
                        lastText = lastText.ToUpper() + " ";
                    }
                    var swear = new string[] {"$", "%", "µ", "!", "§"};
                    var swearCount = Math.Max(PlayerMistakeCount - 2, 0) * 2;
                    for (int i = 0; i < swearCount; i++)
                    {
                        lastText += swear[UnityEngine.Random.Range(0, swear.Length)];
                    }
                    StartCoroutine(
                        WriteLineIn(
                            "red",
                            last.Author,
                            lastText,
                            .8f));

                }
            }

            Input.text = "";
            Input.ActivateInputField();
        }

        public string GetTimeString()
        {
            var dayTime = GameManager.Instance.Game.DayTime;
            string time = string.Format("{0:00}:{1:00} {2}",
                    dayTime.Hours < 12 ? dayTime.Hours : dayTime.Hours - 12,
                    dayTime.Minutes,
                    dayTime.Hours <= 12 ? "AM" : "PM");
            return time;
        }

        public IEnumerator WriteLineIn(string userColor, string user, string text, float delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);
            WriteLine(GetTimeString(), userColor, user, text);
        }

        public void WriteLine(string time, string userColor, string user, string text)
        {
            ChatText.text +=
                string.Format(
                    "<color=grey>{0}</color>\t<color={1}>{2}</color>: {3}" + Environment.NewLine,
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
                CustomerProgress.Refresh(customer);
                TitleText.text = Contact.Name + " (" + Mathf.Round(customer.Satisfaction) + ")";
                return;
            }

            TitleText.text = Contact.Name;
        }
    }
}