using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckMousePos : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<bool> onMouseOver;
    public void OnPointerEnter(PointerEventData eventData)
    {
        onMouseOver?.Invoke(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseOver?.Invoke(false);
    }

    public void ForceHover(bool hovering)
    {
        onMouseOver?.Invoke(hovering);
    }
    
}
