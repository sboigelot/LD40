using System;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ChatWindowController : WindowController
    {
        public Text ChatText;
        public InputField Input;

        public Customer Customer;
        public Issue CurrentIssue;

        protected override void OnOpen(object context)
        {
            Customer = (Customer)context;
            NextIssue();
        }

        private void NextIssue()
        {
            CurrentIssue = GameManager.Instance.Game.Issues[UnityEngine.Random.Range(0, 99)];
            WriteLine(GetTimeString(), "red", "Customer", CurrentIssue.Question.ToString());
        }

        public void Send()
        {
            if (string.IsNullOrEmpty(Input.text.Trim()))
            {
                return;
            }

            string time = GetTimeString();
            WriteLine(time, "green","Player", Input.text);

            if (ValidateAnswer(Input.text) >= .9f)
            {
                NextIssue();
            }
            else
            {
                WriteLine(time, "red", "Customer", CurrentIssue.Question.ToString().ToUpper());
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

        private float ValidateAnswer(string answer)
        {
            if (CurrentIssue == null)
            {
                return 1f;
            }

            var correctAnswer = CurrentIssue.Question.ToString();
            answer = answer.ToLower();

            if (correctAnswer == answer)
            {
                return 1f;
            }

            int validChar = 0;
            for (int i = 0; i < correctAnswer.Length; i++)
            {
                //TODO Split and calculate per words
                //if there is difference in length of words, validate witch contains of chars
            }

            return 1f;
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