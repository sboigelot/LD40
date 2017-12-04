using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class CustomSubmit : MonoBehaviour, IPointerClickHandler
    {
        public InputField Field;
        private bool wasFocused;
        
        public UnityEvent OnEnter;

        public UnityEvent OnClick;

        private void Update()
        {
            if (wasFocused && Input.GetKeyDown(KeyCode.Return))
            {
                Submit(Field.text);
            }

            wasFocused = Field.isFocused;
        }

        private void Submit(string text)
        {
            if (OnEnter != null)
            {
                OnEnter.Invoke();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnClick != null)
            {
                OnClick.Invoke();
            }
        }
    }
}