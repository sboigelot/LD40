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
            FocusWindowAndInput();
        }

        public virtual void FocusWindowAndInput()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        public void CloseWindow()
        {
            OnClose();
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
            FocusWindowAndInput();
            OnOpen();
        }

        public void OpenContextualWindow(object context)
        {
            FocusWindowAndInput();
            OnOpen(context);
        }
    }
}