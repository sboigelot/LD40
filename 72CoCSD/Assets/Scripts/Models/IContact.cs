using Assets.Scripts.UI;

namespace Assets.Scripts.Models
{
    public interface IContact
    {
        string Name { get; }
        string NextForcedPlayerInput { get; }
        float Read(string playerText);
        ChatLine Speak();
        ChatLine GetLastSentence();
        void QuitSatified();
        ChatWindowController ChatWindow { get; set; }
    }
}