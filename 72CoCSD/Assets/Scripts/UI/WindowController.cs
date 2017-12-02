using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowController : MonoBehaviour, IPointerDownHandler
{
    public bool DestroyOnClose;

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
}