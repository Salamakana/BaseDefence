using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : BaseUIButton, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isActive)
        {
            unityEvent.Invoke();
        }
    }
}
