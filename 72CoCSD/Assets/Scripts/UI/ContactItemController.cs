using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ContactItemController : MonoBehaviour
    {
        public ContactBase Contact;
        public Text CustomerNameText;

        public CustomerProgressController CustomerProgressController;

        public void SetupForContact(ContactBase customer)
        {
            Contact = customer;
            customer.ContactItemController = this;
        }

        public void OpenChat()
        {
            if (Contact.ChatWindow == null)
            {
                Contact.ChatWindow = DesktopController.Instance.OpenChatWindow(Contact);
            }
            else
            {
                Contact.ChatWindow.FocusWindowAndInput();
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
    }
}
