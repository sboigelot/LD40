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
            //only support one dialog for now
            CurrentDialog = Dialogs.FirstOrDefault();

            if (CurrentDialog == null || CurrentDialog.MoveNext())
            {
                return "";
            }

            Dialogs.RemoveAt(0);
            NextForcedPlayerInput = CurrentDialog.CurrentLine.ForcedAnswer;
            return CurrentDialog.CurrentLine.Question;
        }

        public string GetLastSentence()
        {
            return CurrentDialog == null || CurrentDialog.CurrentLine == null ? "" : CurrentDialog.CurrentLine.Question;
        }

        public void QuitSatified()
        {
            
        }
    }
}