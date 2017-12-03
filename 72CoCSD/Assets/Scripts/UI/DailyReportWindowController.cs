using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class DailyReportWindowController : WindowController
    {
        public Transform ReportsPanel;
        public GameObject ReportTemplate;
        public Scrollbar Scrollbar;

        public DailyReportWindowController()
        {
            Instance = this;
        }

        public static DailyReportWindowController Instance { get; set; }

        protected override void OnOpen()
        {
            Scrollbar.value = 1;
            Rebuild();
        }

        public void Rebuild()
        {
            ReportsPanel.ClearChildren(2);

            var reports = GameManager.Instance.Game.DailyReports;
            float runningSum = 0;
            var ReportTextFormat = ReportsPanel.GetChild(1).GetComponentInChildren<Text>().text;

            foreach (var report in reports.OrderBy(i => i.Day))
            {
                var reportCard = Instantiate(ReportTemplate, ReportsPanel);
                runningSum += report.TotalSatisfaction;
                reportCard.GetComponentInChildren<Text>().text =
                    string.Format(
                        ReportTextFormat.Replace("<br>", "\n"),
                        report.Day,
                        report.TotalSatisfaction,
                        report.ServeCustomers,
                        report.FailedCustomers,
                        runningSum);
                reportCard.SetActive(true);
            }
        }
    }
}