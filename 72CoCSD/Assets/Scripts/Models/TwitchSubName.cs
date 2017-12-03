using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class TwitchSubName
    {
        [XmlAttribute]
        public string Name;
    }
}