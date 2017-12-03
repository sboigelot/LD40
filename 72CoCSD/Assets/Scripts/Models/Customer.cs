using System;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Customer : IContact
    {
        public string Name { get; set; }

        public int IssueLeft;

        public float Satisfaction { get; set; }

        public string NextForcedPlayerInput { get; set; }

        public CustomerSpawn Prototype;

        public Issue CurrentIssue;

        public string Speak()
        {
            IssueLeft--;
            if (IssueLeft < 0)
            {
                return string.Format("<color=blue>{0} left the chat</color>", Name);
            }

            CurrentIssue = GameManager.Instance.Game.Issues[UnityEngine.Random.Range(0, 99)];
            return CurrentIssue.Question.ToString();
        }

        public float Read(string playerText)
        {
            if (CurrentIssue == null)
            {
                return 1f;
            }

            var correctAnswer = CurrentIssue.Answer.ToString();
            playerText = playerText.ToLower();

            if (correctAnswer == playerText)
            {
                return 1f;
            }

            int validChar = 0;
            for (int i = 0; i < correctAnswer.Length; i++)
            {
                //TODO Split and calculate per words
                //if there is difference in length of words, validate witch contains of chars
            }

            return 0f;
        }

        public string GetLastSentence()
        {
            return CurrentIssue == null ? "" : CurrentIssue.Question.ToString();
        }
    }
}