using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class Sentence
    {
        public List<Word> Words;

        public override string ToString()
        {
            return string.Join(" ", Words.Select(w=>w.Text).ToArray());
        }

        public Sentence(List<Word> vocabulary, int minWord, int maxWord)
        {
            Words = new List<Word>();
            int wordCount = UnityEngine.Random.Range(minWord, maxWord);

            for (int i = 0; i < wordCount; i++)
            {
                Words.Add(vocabulary[UnityEngine.Random.Range(0, vocabulary.Count - 1)]);
            }
        }

        public float Complexity
        {
            get { return Words.Sum(w => w.Complexity); }
        }
    }
}