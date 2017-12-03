using System;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class DailyReport
    {
        public int Day;
        public float TotalSatisfaction;
        public int ServeCustomers;
        public int FailedCustomers;
    }
}