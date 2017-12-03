using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ContactItemController : MonoBehaviour
    {
        public IContact Contact;
        public Text CustomerNameText;

        public ChatWindowController ChatWindow;

        public CustomerProgressController CustomerProgressController;

        public void SetupForCustomer(Customer customer)
        {
            Contact = customer;
            customer.ContactItemController = this;
        }

        public void OpenChat()
        {
            if (ChatWindow == null)
            {
                ChatWindow = DesktopController.Instance.OpenChatWindow(Contact);
            }
            else
            {
                ChatWindow.FocusWindowAndInput();
            }
        }

        public void Update()
        {
            if (Contact == null)
            {
                return;
            }

            var customer = Contact as Customer;
            if (customer != null)
            {
                CustomerNameText.text = customer.Name;
                CustomerProgressController.Refresh(customer);
                return;
            }
            
            CustomerNameText.text = Contact.Name;
        }

        public void Disconect()
        {
            if (Contact == null)
            {
                return;
            }

            var customer = Contact as Customer;
            if (customer != null)
            {
                if (ChatWindow != null)
                {
                    ChatWindow.WriteLine(
                        ChatWindow.GetTimeString(),
                        "blue",
                        "System",
                        string.Format("<color=blue>{0} left the chat with a satisfaction of {1}</color>", Contact.Name,
                            Mathf.Round(customer.Satisfaction)));
                }
                else
                {
                    Debug.LogWarning("ChatWindow not found on disconecting customer "+customer.Name);
                }
                GameManager.Instance.Game.CustomerQueue.Remove(customer);
            }

            Contact = null;
            ContactWindowController.Instance.Rebuild();
        }
    }
}
