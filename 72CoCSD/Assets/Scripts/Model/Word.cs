using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class Word
    {
        private List<string> wordpart_v = new List<string> { "a", "e", "i", "o", "u", "y" };

        private List<string> wordpart_c = new List<string>
        {
            "z",
            "r",
            "t",
            "p",
            "q",
            "d",
            "f",
            "g",
            "h",
            "j",
            "k",
            "l",
            "m",
            "w",
            "x",
            "c",
            "v",
            "b",
            "n",
            "ll",
            "kk",
            "bn",
            "ch",
            "hr"
        };
        public string Text;
        public float LetterSwapPotential;

        public float Complexity
        {
            get { return Text.Length * LetterSwapPotential; }
        }

        public Word()
        {
            GenerateWordText();
            LetterSwapPotential = UnityEngine.Random.Range(0, 1f);
        }
        
        private void GenerateWordText()
        {
            int component = UnityEngine.Random.Range(3, 7);

            int vCount = 0;
            int cCount = 0;

            for (int j = 0; j <= component; j++)
            {
                bool preferededV = UnityEngine.Random.Range(0, 1f) > 0.35f;
                preferededV = preferededV && !(vCount > 2);
                preferededV = preferededV || cCount > 1;

                if (preferededV)
                {
                    vCount++;
                    cCount = 0;
                    Text += wordpart_v[UnityEngine.Random.Range(0, wordpart_v.Count - 1)];
                }
                else
                {
                    cCount++;
                    vCount = 0;
                    Text += wordpart_c[UnityEngine.Random.Range(0, wordpart_c.Count - 1)];
                }
            }
        }
    }
}