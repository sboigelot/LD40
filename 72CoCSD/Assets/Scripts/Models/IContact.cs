namespace Assets.Scripts.Models
{
    public interface IContact
    {
        string Name { get; }
        string NextForcedPlayerInput { get; }
        float Read(string playerText);
        string Speak();
        string GetLastSentence();
        void QuitSatified();
    }
}