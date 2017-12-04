using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class EndGamePanel : MonoBehaviour
    {
        private string template;
        public Text text;

        public void Open()
        {
            if (string.IsNullOrEmpty(template))
            {
                template = text.text;
            }
            text.text = string.Format(template, GameManager.Instance.Game.DailyReports.Last().TotalSatisfaction);
            this.gameObject.SetActive(true);
        }
    }
}