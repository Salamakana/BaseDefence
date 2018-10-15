using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BaseUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent unityEvent;

    private Image background;
    private Image icon;

    private Vector2 defaultScale;
    private Vector2 activeScale = new Vector2(1.2f, 1.2f);
    private Color activeColor;
    private Color disableColor = Color.grey;

    protected bool isActive = true;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        background = transform.Find("Background").GetComponent<Image>();
        icon = background.transform.Find("Icon").GetComponent<Image>();

        defaultScale = transform.localScale;
        activeColor = icon.color;
    }
 
    public void SetActiveState(bool newActiveState)
    {
        isActive = newActiveState;
        icon.color = isActive ? activeColor : disableColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = activeScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = defaultScale;
    }
}
