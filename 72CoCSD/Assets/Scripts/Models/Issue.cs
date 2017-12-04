using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Issue
    {
        public Sentence Question;

        public Sentence Answer;

        public bool Unlocked;

        public float Complexity
        {
            get
            {
                return Question.Complexity + 3 * Answer.Complexity;
            }
        }

        public void Initialise(List<Word> vocabulary)
        {
            Question = new Sentence(vocabulary, 2, 6);

            var onWordAnswerIssueCount = GameManager.Instance.Game.Issues.Count(i => i.Answer.Words.Count == 1);
            Answer = new Sentence(vocabulary,
                onWordAnswerIssueCount <= 7 ? 1 : 2, 4);
            Unlocked = false;
        }
    }
}