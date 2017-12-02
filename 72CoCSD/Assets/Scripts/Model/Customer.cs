using System;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class Customer
    {
        public string Name;
        public int IssueLeft;
        public float Satisfaction;
        public CustomerSpawn Prototype;
    }
}