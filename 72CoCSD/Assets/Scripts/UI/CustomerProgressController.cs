using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class CustomerProgressController : MonoBehaviour
    {
        public Slider Slider;
        public Image Fill;

        public void Refresh(Customer customer)
        {
            var max = customer.Prototype.StartingSatisfaction;
            var min = customer.Satisfaction;
            Slider.maxValue = max;
            Slider.minValue = 0;
            Slider.value = min;

            if (min < max / 3)
            {
                Fill.color = Color.red;
            }
            else if (min < max / 2)
            {
                Fill.color = Color.yellow;
            }
            else
            {
                Fill.color = Color.green;
            }
        }

    }
}