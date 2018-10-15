using System.Collections.Generic;
using UnityEngine;

public abstract class SelectableObject : MonoBehaviour
{
    #region VARIABLES

    private Stack<Collider> hitColliders = new Stack<Collider>();
    private Collider hitCollider;
    private Vector3 LastPlacementPosition;

    #endregion VARIABLES

    #region PROPERTIES

    public Rigidbody Rb
    {
        get;
        private set;
    }
    public float SelectableRadius
    {
        get;
        protected set;
    }
    public bool IsCollision
    {
        get
        {
            return hitColliders.Count > 0 ? true : false;
        }
    }
    public bool IsFirstPlacement
    {
        get;
        private set;
    }

    #endregion PROPERTIES

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        hitCollider = GetComponent<Collider>();
    }

    protected virtual void OnEnable()
    {
        Rb.isKinematic = false;
        IsFirstPlacement = true;
    }

    protected virtual void OnDisable()
    {     
        hitColliders.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hitCollider.isTrigger)
            hitColliders.Push(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (hitCollider.isTrigger && IsCollision)
            hitColliders.Pop();
    }

    public void BeingSelected()
    {
        Rb.isKinematic = false;

        if(IsCollision)
        hitColliders.Clear();
    }

    public void SuccessfulPlacement()
    {
        if (IsFirstPlacement)
        {
            IsFirstPlacement = false;
            PlayerStats.Instance.AddEnergy(-InputManager.Instance.CurrentCastableButtonSelected.CastableBlueprint.EnergyCost);
        }

        Rb.isKinematic = true;
        LastPlacementPosition = transform.position;
    }

    public void ReplaceObject()
    {
        Rb.isKinematic = true;
        transform.position = LastPlacementPosition;
    }
}
