using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Word
    {
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
            string lastC = "";

            for (int j = 0; j <= component; j++)
            {
                bool preferededV = UnityEngine.Random.Range(0, 1f) > 0.35f;
                preferededV = preferededV && !(vCount > 2);
                preferededV = preferededV || cCount > 0;

                if (preferededV)
                {
                    vCount++;
                    cCount = 0;
                    var c = RandomPart(PrototypeManager.Instance.VowelPrototypes).Text;
                    while (lastC == c)
                    {
                        c = RandomPart(PrototypeManager.Instance.VowelPrototypes).Text;
                    }
                    lastC = c;
                    Text += c;
                }
                else
                {
                    cCount++;
                    vCount = 0;
                    Text += RandomPart(PrototypeManager.Instance.ConsonantPrototypes).Text;
                }
            }
        }
    }
}