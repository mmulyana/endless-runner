using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonJump : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.instance.player.playerJump();
    }
}
