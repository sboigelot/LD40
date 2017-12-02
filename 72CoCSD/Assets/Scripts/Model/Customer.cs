using System;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class Customer
    {
        public string Name;
        public float StartingIssue;
        public float IssueLeft;
        public float LanguageProficiency;
        public float StartingSatisfaction;
        public float Satisfaction;
    }
}