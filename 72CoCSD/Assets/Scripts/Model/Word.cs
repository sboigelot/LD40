using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class Word
    {
        private readonly List<WordPart> wordpart_v = new List<WordPart>
        {
            new WordPart("a", 2f),
            new WordPart("e", 3f),
            new WordPart("i", 2f),
            new WordPart("o", 1.5f),
            new WordPart("u", 1.5f),
            new WordPart("y", 0.7f),
        };

        private readonly List<WordPart> wordpart_c = new List<WordPart>
        {
            new WordPart("z", .2f),
            new WordPart("r", 1f),
            new WordPart("t", 1.2f),
            new WordPart("p", 1.2f),
            new WordPart("q", 1f),
            new WordPart("d", 1.2f),
            new WordPart("f", 1f),
            new WordPart("g", 1.1f),
            new WordPart("h", 1f),
            new WordPart("j", 1.2f),
            new WordPart("k", 1.2f),
            new WordPart("l", 1.3f),
            new WordPart("m", 1.3f),
            new WordPart("w", 0.7f),
            new WordPart("x", 0.6f),
            new WordPart("c", 1.2f),
            new WordPart("v", 1.2f),
            new WordPart("b", 1.2f),
            new WordPart("n", 1.3f),
            new WordPart("ll", .7f),
            new WordPart("kk", .3f),
            new WordPart("gg", .3f),
            new WordPart("ch", .7f),
            new WordPart("ss", 1f)
        };

        public WordPart RandomPart(List<WordPart> list)
        {
            var totalPonderation = list.Sum(p => p.Ponderation);
            var randomValue = UnityEngine.Random.Range(0, totalPonderation);
            var randomizedList = list.OrderBy(p => UnityEngine.Random.Range(0, 1f));
            foreach (var wordPart in randomizedList)
            {
                randomValue -= wordPart.Ponderation;
                if (randomValue <= 0)
                {
                    return wordPart;
                }
            }
            return randomizedList.Last();
        }

        public string Text;
        public float LetterSwapPotential;

        public float Complexity
        {
            get { return Text.Length * LetterSwapPotential; }
        }

        public Word()
        {
            GenerateWordText();
            LetterSwapPotential = 1f; //Disabled UnityEngine.Random.Range(0, 1f);
        }
        
        private void GenerateWordText()
        {
            AnimationCurve wordLengths = GameManager.Instance.WordLengths;
            int component = (int)Math.Round(wordLengths.Evaluate(UnityEngine.Random.Range(0, 1f)));

            int vCount = 0;
            int cCount = 0;

            for (int j = 0; j <= component; j++)
            {
                bool preferededV = UnityEngine.Random.Range(0, 1f) > 0.35f;
                preferededV = preferededV && !(vCount > 2);
                preferededV = preferededV || cCount > 0;

                if (preferededV)
                {
                    vCount++;
                    cCount = 0;
                    Text += RandomPart(wordpart_v).Text;
                }
                else
                {
                    cCount++;
                    vCount = 0;
                    Text += RandomPart(wordpart_c).Text;
                }
            }
        }
    }
}