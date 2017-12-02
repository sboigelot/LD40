using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class Issue
    {
        public Sentence Question;
        //public float QuestionPotentialOrderDivergence;

        public Sentence Answer;
        //public bool AnswerOrderMatter;
        //public float AnswerOrderDivergenceAcceptance;

        public bool Unlocked;

        public float Complexity
        {
            get { return Question.Complexity + 4 * Answer.Complexity; }
        }

        public void Initialise(List<Word> vocabulary)
        {
            Question = new Sentence(vocabulary, 2, 6);
            Answer = new Sentence(vocabulary, 1, 4);
            Unlocked = false;
        }
    }
}