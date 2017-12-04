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
        public ContactBase ContactBase;
        public int OverrideMaxLine;

        protected override void OnOpen(object context)
        {
            Input.onValidateInput += OnValidateInput;
            ChatText.text = "";
            ContactBase = (ContactBase)context;
            CustomerProgressPanel.SetActive(ContactBase is Customer);
            ContactBase.ChatWindow = this;
            WriteNextLine(0);
        }

        protected override void OnClose()
        {
            if (ContactBase is Collegue)
            {
                GameManager.Instance.Game.PauseHandle--;
            }
            Destroy(gameObject);
        }

        private char OnValidateInput(string text, int charIndex, char addedChar)
        {
            var forced = ContactBase.NextForcedPlayerInput;

            if (GameManager.Instance.Game.Paused && 
                ContactBase is Customer)
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
            var nextLine = ContactBase.Speak();

            if (nextLine == null)
            {
                ContactBase.QuitSatified();
                Input.enabled = false;
                return;
            }

            StartCoroutine(
                WriteLineIn("red", nextLine.Author, nextLine.Text, delayInSeconds));
        }

        public void Send()
        {
            string playerText = Input.text.Trim();

            if (string.IsNullOrEmpty(Input.text.Trim()) || ContactBase == null)
            {
                return;
            }

            string time = GetTimeString();
            var effectivenes = ContactBase.Read(playerText);
            var understood = effectivenes >= PrototypeManager.Instance.GameSettings.AnswerDeviationTolerance;

            var cheat = true;
            if (cheat)
            {
                understood = true;
            }

            var forced = ContactBase.NextForcedPlayerInput;
            string text = !string.IsNullOrEmpty(forced) ? forced : 
                ContactBase is Customer
                    ? string.Format("{0}\t\t<i>(<color={1}>{2}</color> {3}% correct)</i>",
                        playerText,
                        understood ? "green" : "red",
                        understood ? cheat? "C" :"V" : "X",
                        Math.Round(effectivenes, 2) * 100)
                    : playerText;


            WriteLine(time, "green","Player", text);
            SoundController.Instance.PlaySound(SoundController.Instance.NewMessage2AudioClip);

            if (understood)
            {
                WriteNextLine(1);
            }
            else
            {
                PlayerMistakeCount++;
                var customer = ContactBase as Customer;
                if (PlayerMistakeCount > 4 && customer != null)
                {
                    customer.RageQuit();
                }
                else
                {
                    var last = ContactBase.GetLastSentence();
                    var lastText = last.Text;
                    if (PlayerMistakeCount > 1)
                    {
                        lastText = lastText.ToUpper() + " ";
                    }
                    var swear = new string[] {"$", "%", "#", "!", "§"};
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
            SoundController.Instance.PlaySound(SoundController.Instance.NewMessage1AudioClip);
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
            if (lineCount >= (OverrideMaxLine != 0 ? OverrideMaxLine : 15))
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
            if (ContactBase == null)
            {
                return;
            }

            var customer = ContactBase as Customer;
            if (customer != null)
            {
                CustomerProgress.Refresh(customer);
                TitleText.text = ContactBase.Name + " (" + Mathf.Round(customer.Satisfaction) + ")";
                return;
            }

            TitleText.text = ContactBase.Name;
        }
    }
}