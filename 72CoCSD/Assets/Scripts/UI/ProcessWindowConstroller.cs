using System;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ProcessWindowConstroller : WindowController
    {
        public Transform IssuesPanel;
        public GameObject IssueTemplate;
        public Scrollbar Scrollbar;

        public string IssueTextFormat = "<color=red>Question:</color> {0}\n<color=green>Answer:</color> {1}";

        public ProcessWindowConstroller()
        {
            Instance = this;
        }

        public static ProcessWindowConstroller Instance { get; set; }

        protected override void OnOpen()
        {
            Scrollbar.value = 1;
            Rebuild();
        }

        public void Rebuild()
        {
            IssuesPanel.ClearChildren(2);

            var issues = GameManager.Instance.Game.Issues.Where(i=>i.Unlocked);
            foreach (var issue in issues.OrderBy(i=>i.Question.ToString()))
            {
                var issueCard = Instantiate(IssueTemplate, IssuesPanel);
                issueCard.GetComponentInChildren<Text>().text =
                    string.Format(IssueTextFormat.Replace("<br>", "\n"), issue.Question, issue.Answer);
                issueCard.GetComponentInChildren<Text>().text += " (cplx: " + issue.Complexity + ")";
                issueCard.SetActive(true);
            }
        }
    }
}