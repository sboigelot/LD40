using System;
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
            newChat.SetActive(true);
        }
    }
}
