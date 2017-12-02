using System;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class CustomerSpawn
    {
        public TimeSpan SpawnTime;
        public bool Spawned;
        public float MinComplexity;
        public float MaxComplexity;
        public int StartingIssue;
        public float LanguageProficiency;
        public float StartingSatisfaction;
    }
}