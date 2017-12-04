using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(WindowController))]
    public class ContactWindowController : MonoBehaviourSingleton<ContactWindowController>
    {
        public Transform CustomerPanel;
        public GameObject CustomerTemplate;

        public void Rebuild()
        {
            var remainingCustomers = ClearDisconectedCustomer();

            var customers = GameManager.Instance.Game.CustomerQueue;
            foreach (var customer in customers.Where(c=>!remainingCustomers.Contains(c)))
            {
                var customerCard = Instantiate(CustomerTemplate, CustomerPanel);
                customerCard.GetComponent<ContactItemController>().SetupForContact(customer);
                customerCard.SetActive(true);
            }
        }

        private List<Customer> ClearDisconectedCustomer()
        {
            var remainingCustomers = new List<Customer>();

            CustomerPanel.ClearChildren(1);
            for (var i = 1; i < transform.childCount; i++)
            {
                var contactCard = transform.GetChild(i);
                var customerController = contactCard.GetComponent<ContactItemController>();
                if (customerController != null)
                {
                    if (customerController.Contact == null)
                    {
                        Object.Destroy(transform.GetChild(i).gameObject);
                    }
                    else
                    {
                        var customer = customerController.Contact as Customer;
                        if (customer != null)
                        {
                            remainingCustomers.Add(customer);
                        }
                    }
                }
            }

            return remainingCustomers;
        }
    }
}