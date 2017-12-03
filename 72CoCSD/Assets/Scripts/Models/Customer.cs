using System;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.UI;
using UnityEngine;

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

        public ContactItemController ContactItemController;

        public string Speak()
        {
            IssueLeft--;
            if (IssueLeft < 0)
            {
                return "";
            }

            var possibleIssues =
                GameManager.Instance.Game.Issues.Where(i =>
                i.Unlocked &&
                i.Complexity <= Prototype.MaxComplexity &&
                i.Complexity >= Prototype.MinComplexity)
                .ToList();

            if (!possibleIssues.Any())
            {
                Debug.LogWarning("No possible issue found for this customer - choose randomly overall");
                possibleIssues = GameManager.Instance.Game.Issues;
                CurrentIssue = possibleIssues[UnityEngine.Random.Range(0, possibleIssues.Count - 1)];
                CurrentIssue.Unlocked = true;
                ProcessWindowConstroller.Instance.Rebuild();
            }
            else
            {
                CurrentIssue = possibleIssues[UnityEngine.Random.Range(0, possibleIssues.Count - 1)];
            }

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
                ImpactSatisfaction(PrototypeManager.Instance.GameSettings.SatisfactionGainPerCorrectAnswer);
                return 1f;
            }

            int validChar = 0;
            for (int i = 0; i < correctAnswer.Length; i++)
            {
                //TODO Split and calculate per words
                //if there is difference in length of words, validate witch contains of chars
            }

            ImpactSatisfaction(PrototypeManager.Instance.GameSettings.SatisfactionGainPerIncorrectAnswer);
            return 0f;
        }

        public string GetLastSentence()
        {
            return CurrentIssue == null ? "" : CurrentIssue.Question.ToString();
        }

        public void ImpactSatisfaction(float value)
        {
            Satisfaction += value;
            if (Satisfaction <= 0)
            {
                RageQuit();
            }
        }

        public void RageQuit()
        {
            if (ContactItemController != null)
            {
                var report = GameManager.Instance.Game.TodayReport;
                report.FailedCustomers++;
                Satisfaction = 0;
                ContactItemController.Disconect();
            }
        }

        public void QuitSatified()
        {
            if (ContactItemController != null)
            {
                var report = GameManager.Instance.Game.TodayReport;
                report.ServeCustomers++;
                report.TotalSatisfaction += Satisfaction;
                ContactItemController.Disconect();
            }
        }
    }
}