using UnityEngine;
using UnityEngine.EventSystems;

public class CastableButton : BaseUIButton, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private CastableBlueprint castableBlueprint;

    public CastableBlueprint CastableBlueprint
    {
        get
        {
            return castableBlueprint;
        }
    }

    private void Start()
    {
        unityEvent.AddListener(StartDragCastable);
    }

    private void StartDragCastable()
    {
        var newSelectableObject = ObjectPoolManager.Instance.GetObjectFromPool(CastableBlueprint.PrefabToCast.name, InputManager.Instance.MouseHitPoint, Quaternion.identity).GetComponent<SelectableObject>();
        if (newSelectableObject != null)
        {
            InputManager.Instance.CurrentlySelectableObject = newSelectableObject;
            InputManager.Instance.ViewRange.ShowViewRadius(newSelectableObject.transform, newSelectableObject.SelectableRadius);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isActive)
        {
            unityEvent.Invoke();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        UIManager.Instance.UpdateSelectedObjectInfo(this, CastableBlueprint);
    }
}
