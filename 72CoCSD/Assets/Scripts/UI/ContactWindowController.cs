using Assets.Scripts.Managers;
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
            CustomerPanel.ClearChildren(1);

            var customers = GameManager.Instance.Game.CustomerQueue;
            foreach (var customer in customers)
            {
                var customerCard = Instantiate(CustomerTemplate, CustomerPanel);
                customerCard.GetComponent<ContactItemController>().SetupForCustomer(customer);
                customerCard.SetActive(true);
            }
        }
    }
}