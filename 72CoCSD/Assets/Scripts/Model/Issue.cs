using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class Issue
    {
        public List<Word> Question;
        public float QuestionPotentialOrderDivergence;

        public List<Word> Answer;
        public bool AnswerOrderMatter;
        public float AnswerOrderDivergenceAcceptance;
    }
}