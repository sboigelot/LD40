using System;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Customer : ContactBase
    {
        public int IssueLeft;

        public float Satisfaction { get; set; }

        public CustomerSpawn Prototype;

        public Issue CurrentIssue;
        
        public override ChatLine Speak()
        {
            IssueLeft--;
            if (IssueLeft < 0)
            {
                return null;
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

            return new ChatLine
            {
                Author = Name,
                Text = CurrentIssue.Question.ToString()
            };
        }

        public override float Read(string playerText)
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

            var playerWords = playerText.Split(' ').ToList();
            var answerWords = CurrentIssue.Answer.Words;

            if (playerWords.Count != answerWords.Count)
            {
                ImpactSatisfaction(PrototypeManager.Instance.GameSettings.SatisfactionGainPerIncorrectAnswer);
                return 0f;
            }

            var effectivenes = 0f;
            for (var index = 0; index < playerWords.Count; index++)
            {
                var playerWord = playerWords[index];
                var answerWord = answerWords[index].Text;
                var wordPonderation = 1f / playerWords.Count;

                if (playerWord == answerWord)
                {
                    effectivenes += wordPonderation;
                }
                else
                {
                    foreach (var answerChar in answerWord)
                    {
                        var charPonderation = wordPonderation / answerWord.Length;
                        if (playerWord.Contains(answerChar))
                        {
                            effectivenes += charPonderation;
                        }
                    }
                }
            }

            if (effectivenes >= PrototypeManager.Instance.GameSettings.AnswerDeviationTolerance)
            {
                ImpactSatisfaction(PrototypeManager.Instance.GameSettings.SatisfactionGainPerCorrectAnswer * effectivenes);
                return effectivenes;
            }

            ImpactSatisfaction(PrototypeManager.Instance.GameSettings.SatisfactionGainPerIncorrectAnswer);
            return effectivenes;
        }

        public override ChatLine GetLastSentence()
        {
            return CurrentIssue == null ? null : new ChatLine
            {
                Author = Name,
                Text = CurrentIssue.Question.ToString()
            };
        }

        public void ImpactSatisfaction(float value)
        {
            Satisfaction += value;
            if (Satisfaction <= 0)
            {
                RageQuit();
            }
        }

        public override void RageQuit()
        {
            if (ContactItemController != null)
            {
                var report = GameManager.Instance.Game.TodayReport;
                report.FailedCustomers++;
                Satisfaction = 0;
            }
            Disconect();
        }

        public override void QuitSatified()
        {
            if (ContactItemController == null)
            {
                return;
            }

            var report = GameManager.Instance.Game.TodayReport;
            report.ServeCustomers++;
            report.TotalSatisfaction += Satisfaction;
            Disconect();
        }

        protected override void Disconect()
        {
            if (ChatWindow != null)
            {
                ChatWindow.WriteLine(
                    ChatWindow.GetTimeString(),
                    "blue",
                    "System",
                    string.Format("<color=blue>{0} left the chat with a satisfaction of {1}</color>", Name,
                        Mathf.Round(Satisfaction)));
            }
            else
            {
                Debug.LogWarning("ChatWindow not found on disconecting customer " + Name);
            }

            ContactItemController.Contact = null;
            GameManager.Instance.Game.CustomerQueue.Remove(this);
            ContactWindowController.Instance.Rebuild();
        }
    }
}