using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSlide : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.instance.player.PlayerSlide();
    }
}
