using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Assets.Scripts.Managers;
using Assets.Scripts.UI;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Collegue : IContact
    {
        [XmlAttribute]
        public string Name { get; set; }

        public string DefaultName { get; set; }

        [XmlAttribute]
        public List<Dialog> Dialogs;

        private Dialog CurrentDialog;

        [XmlIgnore]
        public string NextForcedPlayerInput { get; set; }

        public float Read(string playerText)
        {
            return 1f;
        }

        public ChatLine Speak()
        {
            CurrentDialog = Dialogs.FirstOrDefault();

            if (CurrentDialog == null || !CurrentDialog.MoveNext())
            {
                Dialogs.RemoveAt(0);
                return null;
            }
            
            NextForcedPlayerInput = CurrentDialog.CurrentLine.ForcedAnswer;
            return new ChatLine
            {
                Author = CurrentDialog.CurrentLine.OverrideSpeaker ?? Name,
                Text = CurrentDialog.CurrentLine.Question
            };
        }

        public ChatLine GetLastSentence()
        {
            return
                CurrentDialog == null || CurrentDialog.CurrentLine == null
                    ? null
                    : new ChatLine
                    {
                        Author = CurrentDialog.CurrentLine.OverrideSpeaker ?? Name,
                        Text = CurrentDialog.CurrentLine.Question
                    };
        }

        public void QuitSatified()
        {
            GameManager.Instance.Game.Paused = false;
            ChatWindow.StartCoroutine(
            ChatWindow.WriteLineIn("blue", "System",
                string.Format("<color=blue>{0} left the chat</color>", Name),
                1));
        }

        public ChatWindowController ChatWindow { get; set; }
    }
}