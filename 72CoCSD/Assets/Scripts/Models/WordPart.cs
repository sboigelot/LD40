using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class WordPart
    {
        [XmlAttribute]
        public string Text;

        [XmlAttribute]
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