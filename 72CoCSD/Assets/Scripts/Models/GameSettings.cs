using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class GameSettings
    {
        [XmlAttribute]
        public float MinutesPerGameTime = 1f;

        [XmlAttribute]
        public float SatisfactionLossPerGameTime = 1f;

        [XmlAttribute]
        public float SatisfactionGainPerCorrectAnswer = 1f;

        [XmlAttribute]
        public float SatisfactionGainPerIncorrectAnswer = 1f;

        [XmlAttribute]
        public int NumberOfPossibleIssues = 100;

        [XmlAttribute]
        public int NumberOfPossibleWords = 100;

        [XmlAttribute]
        public float AnswerDeviationTolerance = .9f;
    }
}