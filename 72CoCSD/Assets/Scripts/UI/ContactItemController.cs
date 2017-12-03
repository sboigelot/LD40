using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ContactItemController : MonoBehaviour
    {
        public Customer Customer;

        public void OpenChat()
        {
            DesktopController.Instance.OpenChatWindow(Customer);
        }
    }
}
