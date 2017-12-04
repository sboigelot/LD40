using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class DesktopController : MonoBehaviourSingleton<DesktopController>
    {
        public Text ClockText;

        public GameObject ChatWindowTemplate;
        public GameObject ProcessWindow;
        public GameObject ContactWindow;
        public GameObject ReportWindow;

        public ChatWindowController OpenChatWindow(ContactBase customer)
        {
            var newChat = Instantiate(ChatWindowTemplate, ChatWindowTemplate.transform.parent);
            
            var moveLeft = 50;
            var otherWindows = ChatWindowTemplate
                .transform
                .parent
                .Cast<Transform>()
                .Where(w => w != newChat.transform)
                .ToList();
            
            while (otherWindows.Any(t =>
                Math.Abs(t.localPosition.x - newChat.transform.localPosition.x) < 5f &&
                Math.Abs(t.localPosition.y - newChat.transform.localPosition.y) < 5f) &&
                moveLeft > 0)
            {
                moveLeft--;
                newChat.transform.Translate(12f, -12f, 0f);
            }

            var chatWindow = newChat.GetComponent<ChatWindowController>();
            chatWindow.OpenContextualWindow(customer);

            return chatWindow;
        }

        public void Update()
        {
            if (GameManager.Instance.Game.Paused)
            {
                ClockText.text = "Game paused";
                return;
            }

            var dayTime = GameManager.Instance.Game.DayTime;

            ClockText.text = string.Format("Day {0} - {1:00}:{2:00} {3}",
                dayTime.Days,
                dayTime.Hours <= 12 ? dayTime.Hours : dayTime.Hours - 12,
                dayTime.Minutes,
                dayTime.Hours <= 12 ? "AM" : "PM");
        }
    }
}
