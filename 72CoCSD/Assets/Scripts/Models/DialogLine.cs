using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class DialogLine
    {
        [XmlAttribute]
        public string OverrideSpeaker;

        [XmlAttribute]
        public string Question;

        [XmlAttribute]
        public string ForcedAnswer;
    }
}