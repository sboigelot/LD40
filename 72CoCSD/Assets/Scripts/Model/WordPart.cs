using System;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class WordPart
    {
        public string Text;
        public float Ponderation;

        public WordPart()
        {
            
        }

        public WordPart(string text, float ponderation)
        {
            Text = text;
            Ponderation = ponderation;
        }
    }
}