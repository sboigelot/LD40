﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Assets.Scripts.UI
{
    public class DesktopController : MonoBehaviour
    {
        public GameObject ChatWindowTemplate;
        public GameObject ProcessWindow;
        public GameObject ContactWindow;
        public GameObject ReportWindow;

        public void OpenChatWindow()
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

            newChat.GetComponent<WindowController>().OpenWindow();
        }
    }
}
