using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class WindowController : MonoBehaviour, IPointerDownHandler
    {
        public bool DestroyOnClose;

        public Action OnOpen;

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.SetAsLastSibling();
        }

        public void CloseWindow()
        {
            if (DestroyOnClose)
            {   
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void OpenWindow()
        {
            if (OnOpen != null)
            {
                OnOpen();
            }

            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }
    }
}