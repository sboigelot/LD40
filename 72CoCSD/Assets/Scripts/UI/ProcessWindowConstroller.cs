using System;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(WindowController))]
    public class ProcessWindowConstroller : MonoBehaviour
    {
        public Transform IssuesPanel;
        public GameObject IssueTemplate;
        public string IssueTextFormat = "<color=red>Question:</color> {0}\n<color=green>Answer:</color> {1}";

        public void Awake()
        {
            //TODO investigate this doesn't work
            //GetComponent<WindowController>().OnOpen = Rebuild;
            Rebuild();
        }

        public void Rebuild()
        {
            IssuesPanel.ClearChildren(2);

            var issues = GameManager.Instance.Game.Issues;
            foreach (var issue in issues.OrderBy(i=>i.Question.ToString()))
            {
                var issueCard = Instantiate(IssueTemplate, IssuesPanel);
                issueCard.GetComponentInChildren<Text>().text =
                    string.Format(IssueTextFormat.Replace("<br>", "\n"), issue.Question, issue.Answer);
                issueCard.SetActive(true);
            }
        }
    }
}