using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class Game
    {
        public float GameTime;
        public bool Paused;

        public List<Issue> Issues;
        public List<Customer> CustomerQueue;
        public List<Word> Words;
        public List<CustomerSpawn> CustomerSpawn;
        
        public bool TutorialCompleted;

        public TimeSpan DayTime;
        public TimeSpan DayStart = new TimeSpan(0, 8, 0, 0);
        public TimeSpan DayEnd = new TimeSpan(5, 17, 0, 0);
        public float MinutesPerGameTime = 1f;

        public void Initialize()
        {
            DayTime = new TimeSpan(DayStart.Ticks);
            GenerateVocabulary();
            GenerateIssues();
            GenerateCustomerSpawn();
            CustomerQueue = new List<Customer>();
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

        private void GenerateCustomerSpawn()
        {
            CustomerSpawn = new List<CustomerSpawn>();
            var hourPerDay = DayEnd.Hours - DayStart.Hours - 1; //no spawn the last hour
            var tickPerDay = (int)TimeSpan.FromHours(hourPerDay).Ticks;

            var issueOfCustomerPerDay = new int[] { 2, 3, 4, 3, 4 };
            var numberOfCustomerPerDay = new int[] {3, 4, 7, 12, 16};
            var complexityOfCustomerPerDay = new float[] {1f, 2f, 2f, 3f, 5f};

            for (int day = 0; day < DayEnd.Days; day++)
            {
                var count = numberOfCustomerPerDay[day];
                var complexity = complexityOfCustomerPerDay[day];
                var issueAvg = issueOfCustomerPerDay[day];

                for (int i = 0; i < count; i++)
                {
                    CustomerSpawn.Add(new CustomerSpawn
                    {
                        LanguageProficiency = 1f,
                        MaxComplexity = complexity,
                        MinComplexity = complexity - 2f,
                        SpawnTime = DayStart
                            .Add(TimeSpan.FromDays(day))
                            .Add(TimeSpan.FromTicks(UnityEngine.Random.Range(0, tickPerDay))),
                        Spawned = false,
                        StartingIssue = UnityEngine.Random.Range(issueAvg - 1, issueAvg + 1),
                        StartingSatisfaction = 100
                    });
                }
            }
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
            var customersToSpawn = CustomerSpawn
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
                CustomerSpawn.Remove(customerSpawn);
                CustomerQueue.Add(customer);
                ContactWindowController.Instance.Rebuild();
            }
        }
    }
}