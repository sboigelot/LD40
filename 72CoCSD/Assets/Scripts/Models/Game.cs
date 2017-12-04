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

        public int PauseHandle
        {
            get { return pauseHandle; }
            set
            {
                pauseHandle = Math.Max(value, 0);
                Paused = pauseHandle > 0;
            }
        }

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
        private int pauseHandle;

        public void Initialize()
        {
            DayTime = new TimeSpan(DayStart.Ticks);
            ResetCustomerSpawns();
            ResetEvents();
            GenerateVocabulary();
            GenerateIssues();
            CustomerQueue = new List<Customer>();
            Paused = false;

            DailyReports = new List<DailyReport>
            {
                new DailyReport()
            };
            //now done by events: UnlockNewIssues(5, 0, 40);
        }

        private void ResetEvents()
        {
            foreach (var eventToReset in PrototypeManager.Instance.Events)
            {
                eventToReset.Triggered = false;
            }
        }

        public void UnlockNewIssues(int count, int minComplexity, int maxComplexity)
        {
            var possibleIssue = Issues.Where(
                issue =>
                    !issue.Unlocked &&
                    issue.Complexity >= minComplexity &&
                    issue.Complexity <= maxComplexity)
                    .ToList();

            for (int i = 0; i < count; i++)
            {
                if (!possibleIssue.Any())
                {
                    possibleIssue = Issues.Where(issue => !issue.Unlocked).ToList();

                    if (!possibleIssue.Any())
                    {
                        Debug.LogError("No more issue available to unlock");
                        return;
                    }
                }

                var selected = possibleIssue[UnityEngine.Random.Range(0, possibleIssue.Count)];
                possibleIssue.Remove(selected);
                selected.Unlocked = true;
            }

            ProcessWindowConstroller.Instance.Rebuild();
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

            string dump = "";
            foreach (var issue in Issues)
            {
                dump += string.Format("{0}: {1} -> {2}", issue.Complexity, issue.Question, issue.Answer) +
                        Environment.NewLine;
            }
            Debug.Log(dump);
        }
        
        public void Update(float deltaTime)
        {
            if (!Paused && PrototypeManager.Instance.Loaded)
            {
                GameTime += Time.deltaTime;
                DayTime = DayTime.Add(TimeSpan.FromMinutes(Time.deltaTime * PrototypeManager.Instance.GameSettings.MinutesPerGameTime));

                if (DayTime.Hours >= DayEnd.Hours)
                {
                    if (EndOfTheDay())
                    {
                        GameManager.Instance.EndGame(!LastDayWasAFailure());
                        return;
                    }
                }

                TriggerEvents();
                SpawnCustomers();
                UpdateCustomerSatisfaction();
            }
        }

        private bool LastDayWasAFailure()
        {
            var lastReport = DailyReports.Last();
            return (float)lastReport.FailedCustomers > (float)lastReport.ServeCustomers / 2f;
        }

        private bool EndOfTheDay()
        {
            DisconectAllClient();
            DailyReportWindowController.Instance.OpenWindow();

            DayTime = new TimeSpan(DayTime.Days + 1, DayStart.Hours, DayStart.Minutes, 0);
            
            if (DayTime.Days == 5 || LastDayWasAFailure())
            {
                return true;
            }
            else
            {
                DailyReports.Add(new DailyReport { Day = DayTime.Days });
                return false;
            }
        }

        private void DisconectAllClient()
        {
            foreach (var customer in CustomerQueue.ToList())
            {
                customer.RageQuit();
            }
        }

        private void TriggerEvents()
        {
            if (PrototypeManager.Instance.Events == null)
                return;

            var eventToTriggers =
                PrototypeManager.Instance.Events
                .Where(cs => !cs.Triggered &&
                             cs.TriggerTime <= DayTime)
                .ToList();

            foreach (var eventToTrigger in eventToTriggers)
            {
                eventToTrigger.Trigger();
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

            var customerOfTheDay =
                PrototypeManager.Instance.CustomerSpawns
                .Where(cs => !cs.Spawned &&
                             cs.SpawnTime.Days == DayTime.Days)
                .ToList();

            if (!customerOfTheDay.Any() && !CustomerQueue.Any())
            {
                var untriggerEvents =
                PrototypeManager.Instance.Events
                .Where(cs => !cs.Triggered &&
                             cs.TriggerTime.Days <= DayTime.Days).ToList();

                if (untriggerEvents.Count == 1)
                {
                    var uEvent = untriggerEvents.Single();
                    var dialogName = uEvent.TriggerDialogName;
                    if (dialogName == "EndOfDay")
                    {
                        uEvent.Trigger();
                        EndOfTheDay();
                    }
                    else if(dialogName == "WinGame")
                    {
                        //TODO implement wining game
                    }
                }
                return;
            }

            var customersToSpawn = customerOfTheDay.Where(cs => cs.SpawnTime <= DayTime).ToList();

            foreach (var customerSpawn in customersToSpawn)
            {
                var customer = customerSpawn.SpawnCustomer();
                CustomerQueue.Add(customer);
                ContactWindowController.Instance.Rebuild();
            }
        }
    }
}