using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected RectTransform rectTransform;
    protected Vector2 defaultPosition;
    protected Vector2 desiredPosition;
    protected Image panelImage;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        panelImage = GetComponent<Image>();
    }

    protected virtual void Start()
    {
        defaultPosition = rectTransform.position;
    }

    public virtual void UpdatePanel()
    {

    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //rectTransform.position = desiredPosition;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //rectTransform.position = defaultPosition;
    }
}
