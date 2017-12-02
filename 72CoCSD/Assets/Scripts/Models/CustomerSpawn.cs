using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class CustomerSpawn
    {
        [XmlAttribute]
        public string Name;

        [XmlIgnore]
        public TimeSpan SpawnTime
        {
            get { return new TimeSpan(SpawnDay,SpawnHour,SpawnMinute,0); }
        }
        
        [XmlAttribute]
        public int SpawnDay;

        [XmlAttribute]
        public int SpawnHour;

        [XmlAttribute]
        public int SpawnMinute;

        [XmlAttribute]
        public bool Spawned;

        [XmlAttribute]
        public float MinComplexity;

        [XmlAttribute]
        public float MaxComplexity;

        [XmlAttribute]
        public int StartingIssue;

        [XmlAttribute]
        public float LanguageProficiency;

        [XmlAttribute]
        public float StartingSatisfaction;
    }
}