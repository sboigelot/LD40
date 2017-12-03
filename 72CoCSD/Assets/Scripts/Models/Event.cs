using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Event
    {
        [XmlIgnore]
        public TimeSpan TriggerTime
        {
            get { return new TimeSpan(TriggerDay, TriggerHour, TriggerMinute, 0); }
        }

        [XmlAttribute]
        public int TriggerDay;

        [XmlAttribute]
        public int TriggerHour;

        [XmlAttribute]
        public int TriggerMinute;

        [XmlAttribute]
        public bool Triggered;

        [XmlAttribute]
        public int UnlockIssue;

        [XmlAttribute]
        public float UnlockIssueMaxComplexity;

        [XmlAttribute]
        public string TriggerDialogName;

        [XmlAttribute]
        public bool OpenReportWindow;

        public void Trigger()
        {
            //TODO
            //UnlockNewIssues(5, 0, 40); and disable in Game.Initialize
            Triggered = true;
        }
    }
}