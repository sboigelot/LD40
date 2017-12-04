using Assets.Scripts.Managers;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public abstract class ContactBase
    {
        public string Name;
        public string NextForcedPlayerInput;

        public abstract float Read(string playerText);
        public abstract ChatLine Speak();
        public abstract ChatLine GetLastSentence();

        public virtual void QuitSatified()
        {
            Disconect();
        }

        public virtual void RageQuit()
        {
            Disconect();
        }

        protected virtual void Disconect()
        {
            ChatWindow.WriteLine(ChatWindow.GetTimeString(), "blue", "System",
                string.Format("<color=blue>{0} left the chat</color>", Name));
        }

        public ChatWindowController ChatWindow;

        public ContactItemController ContactItemController;
    }
}