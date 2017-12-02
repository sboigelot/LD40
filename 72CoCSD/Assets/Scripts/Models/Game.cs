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
        
        public bool TutorialCompleted;

        public TimeSpan DayTime;
        public TimeSpan DayStart = new TimeSpan(0, 8, 0, 0);
        public TimeSpan DayEnd = new TimeSpan(5, 17, 0, 0);
        public float MinutesPerGameTime = 1f;

        public void Initialize()
        {
            DayTime = new TimeSpan(DayStart.Ticks);
            ResetCustomerSpawns();
            GenerateVocabulary();
            GenerateIssues();
            CustomerQueue = new List<Customer>();
            Paused = false;
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
            for (int i = 0; i < 100; i++)
            {
                Words.Add(new Word());
            }
            Words = Words.OrderBy(w => w.Complexity).ToList();
        }

        private void GenerateIssues()
        {
            Issues = new List<Issue>();
            for (int i = 0; i < 100; i++)
            {
                var issue = new Issue();
                issue.Initialise(Words);
                Issues.Add(issue);
            }
            Issues = Issues.OrderBy(w => w.Complexity).ToList();
        }
        
        public void Update(float deltaTime)
        {
            if (!Paused)
            {
                GameTime += Time.deltaTime;
                DayTime = DayTime.Add(TimeSpan.FromMinutes(Time.deltaTime * MinutesPerGameTime));
                SpawnCustomers();
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
                var customer = new Customer
                {
                    Name = "Customer",
                    IssueLeft = customerSpawn.StartingIssue,
                    Satisfaction = customerSpawn.StartingSatisfaction,
                    Prototype = customerSpawn
                };
                customerSpawn.Spawned = true;
                CustomerQueue.Add(customer);
                ContactWindowController.Instance.Rebuild();
            }
        }
    }
}