using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Game
    {
        public float GameTime;
        public bool Paused = true;

        public List<Issue> Issues;
        public List<Customer> CustomerQueue;
        public List<Word> Words;
        public List<DailyReport> DailyReports;

        public DailyReport TodayReport
        {
            get { return DailyReports.Last(); }
        }

        public bool TutorialCompleted;

        public TimeSpan DayTime;
        public TimeSpan DayStart = new TimeSpan(0, 8, 0, 0);
        public TimeSpan DayEnd = new TimeSpan(5, 17, 0, 0);
        
        public void Initialize()
        {
            DayTime = new TimeSpan(DayStart.Ticks);
            ResetCustomerSpawns();
            GenerateVocabulary();
            GenerateIssues();
            CustomerQueue = new List<Customer>();
            Paused = false;

            DailyReports = new List<DailyReport>
            {
                new DailyReport()
            };
        }

        private void ResetCustomerSpawns()
        {
            foreach (var customerSpawn in PrototypeManager.Instance.CustomerSpawns)
            {
                customerSpawn.Spawned = false;
            }
        }

        private void GenerateVocabulary()
        {
            Words = new List<Word>();
            for (int i = 0; i < PrototypeManager.Instance.GameSettings.NumberOfPossibleWords; i++)
            {
                Words.Add(new Word());
            }
            Words = Words.OrderBy(w => w.Complexity).ToList();
        }

        private void GenerateIssues()
        {
            Issues = new List<Issue>();
            for (int i = 0; i < PrototypeManager.Instance.GameSettings.NumberOfPossibleIssues; i++)
            {
                var issue = new Issue();
                issue.Initialise(Words);
                Issues.Add(issue);
            }
            Issues = Issues.OrderBy(w => w.Complexity).ToList();
        }
        
        public void Update(float deltaTime)
        {
            if (!Paused && PrototypeManager.Instance.Loaded)
            {
                GameTime += Time.deltaTime;
                DayTime = DayTime.Add(TimeSpan.FromMinutes(Time.deltaTime * PrototypeManager.Instance.GameSettings.MinutesPerGameTime));

                if (DayTime.Hours >= DayEnd.Hours)
                {
                    EndOfTheDay();
                }

                SpawnCustomers();
                UpdateCustomerSatisfaction();
            }
        }

        private void EndOfTheDay()
        {
            DisconectAllClient();
            DesktopController.Instance.ShowDailyReport(DayTime.Days);
            DayTime = new TimeSpan(DayTime.Days + 1, DayStart.Hours, DayStart.Minutes, 0);
            DailyReports.Add(new DailyReport { Day = DayTime.Days });
        }

        private void DisconectAllClient()
        {
            foreach (var customer in CustomerQueue)
            {
                customer.RageQuit();
            }
        }

        private void UpdateCustomerSatisfaction()
        {
            foreach (var customer in CustomerQueue.ToList())
            {
                customer.ImpactSatisfaction(-Time.deltaTime * PrototypeManager.Instance.GameSettings.SatisfactionLossPerGameTime);
            }
        }

        private void SpawnCustomers()
        {
            if(PrototypeManager.Instance.CustomerSpawns == null)
                return;

            var customersToSpawn = 
                PrototypeManager.Instance.CustomerSpawns
                .Where(cs => !cs.Spawned &&
                             cs.SpawnTime <= DayTime)
                .ToList();

            foreach (var customerSpawn in customersToSpawn)
            {
                var customer = customerSpawn.SpawnCustomer();
                CustomerQueue.Add(customer);
                ContactWindowController.Instance.Rebuild();
            }
        }
    }
}