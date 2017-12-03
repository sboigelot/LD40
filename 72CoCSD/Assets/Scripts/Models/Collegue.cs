using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Collegue : IContact
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public List<Dialog> Dialogs;

        private Dialog CurrentDialog;

        [XmlIgnore]
        public string NextForcedPlayerInput { get; set; }

        public float Read(string playerText)
        {
            return 1f;
        }

        public string Speak()
        {
            CurrentDialog = Dialogs.FirstOrDefault();
            if (CurrentDialog == null)
            {
                return "";
            }

            Dialogs.RemoveAt(0);
            NextForcedPlayerInput = CurrentDialog.ForcedAnswer;
            return CurrentDialog.Question;
        }

        public string GetLastSentence()
        {
            return CurrentDialog == null ? "" : CurrentDialog.Question;
        }
    }
}