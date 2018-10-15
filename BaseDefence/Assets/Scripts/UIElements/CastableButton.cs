using UnityEngine;
using UnityEngine.EventSystems;

public class CastableButton : BaseUIButton, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private Castable Castable;
    public int CastCost
    {
        get;
        private set;
    }

    private void Start()
    {
        unityEvent.AddListener(DragCastable);
        CastCost = Castable.castCost;
    }

    private void DragCastable()
    {
        var newSelectableObject = ObjectPoolManager.Instance.GetObjectFromPool(Castable.PrefabToCast.name, InputManager.Instance.MouseHitPoint, Quaternion.identity).GetComponent<SelectableObject>();
        if (newSelectableObject != null)
        {
            InputManager.Instance.SelectableObject = newSelectableObject;
            InputManager.Instance.ViewRange.ShowViewRadius(newSelectableObject.transform, newSelectableObject.ViewRadius);
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
}
