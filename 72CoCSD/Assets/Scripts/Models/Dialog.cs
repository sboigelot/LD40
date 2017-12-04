using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Assets.Scripts.Managers;
using Assets.Scripts.UI;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Dialog
    {
        [XmlAttribute]
        public string Id;

        [XmlAttribute]
        public string CollegueName;

        [XmlAttribute]
        public bool PauseGame;

        [XmlElement("Line")]
        public List<DialogLine> Lines;

        [XmlIgnore]
        public DialogLine CurrentLine;

        [XmlIgnore]
        public int index = -1;

        [XmlAttribute]
        public int OverrideMaxLine;

        public bool MoveNext()
        {
            index++;

            if (Lines == null || index >= Lines.Count)
            {
                CurrentLine = null;
                return false;
            }

            CurrentLine = Lines[index];
            return true;
        }

        public void OpenChat()
        {
            if (PauseGame)
            {
                GameManager.Instance.Game.Paused = true;
            }

            var chatWindow = DesktopController.Instance.OpenChatWindow(new Collegue
            {
                Name = CollegueName,
                Dialogs = new List<Dialog> {this}
            });
            chatWindow.OverrideMaxLine = OverrideMaxLine;
        }
    }
}