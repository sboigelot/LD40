using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class Game
    {
        public float GameTime;
        public bool Paused;

        public List<Issue> Issues;
        public List<Customer> CustomerQueue;
        public List<Word> Words;

        public float TimeToNextCustomer;

        public bool TutorialCompleted;

        public void Initialize()
        {
            GenerateVocabulary();
            GenerateIssues(); 
        }

        private void GenerateVocabulary()
        {
            Words = new List<Word>();
            for (int i = 0; i < 100; i++)
            {
                Words.Add(new Word());
            }
            Words = Words.OrderBy(w => w.Complexity).ToList();
        }

        private void GenerateIssues()
        {
            Issues = new List<Issue>();
            for (int i = 0; i < 100; i++)
            {
                var issue = new Issue();
                issue.Initialise(Words);
                Issues.Add(issue);
            }
            Issues = Issues.OrderBy(w => w.Complexity).ToList();
        }
    }
}