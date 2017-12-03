using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class WindowController : MonoBehaviour, IPointerDownHandler
    {
        public bool DestroyOnClose;

        protected virtual void OnOpen()
        {

        }

        protected virtual void OnOpen(object context)
        {

        }

        protected virtual void OnClose()
        {

        }

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
            OnOpen();
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        public void OpenContextualWindow(object context)
        {
            OnOpen(context);
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }
    }
}