using System;
using System.Xml.Serialization;
using Assets.Scripts.Managers;
using Assets.Scripts.UI;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class CustomerSpawn
    {
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

        public Customer SpawnCustomer()
        {
            SoundController.Instance.PlaySound(SoundController.Instance.NewContact1AudioClip);
            var twitchSubNames = PrototypeManager.Instance.TwitchSubNames;
            var customer = new Customer
            {
                Name = twitchSubNames[UnityEngine.Random.Range(0, twitchSubNames.Count - 1)].Name,
                IssueLeft = StartingIssue,
                Satisfaction = StartingSatisfaction,
                Prototype = this
            };
            Spawned = true;
            return customer;
        }
    }
}