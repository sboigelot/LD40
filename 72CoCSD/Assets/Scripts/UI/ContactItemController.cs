using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ContactItemController : MonoBehaviour
    {
        public Customer Customer;
        public Text CustomerNameText;
        
        public void SetupForCustomer(Customer customer)
        {
            Customer = customer;
            CustomerNameText.name = customer.Name;
        }

        public void OpenChat()
        {
            DesktopController.Instance.OpenChatWindow(Customer);
        }
    }
}
