using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{   
    private Stack<Collider> hitColliders = new Stack<Collider>();

    private BaseTurret baseTurret;
    private Collider hitCollider;
    private Vector3 LastPlacementPosition;

    public Rigidbody Rb
    {
        get;
        private set;
    }

    public float ViewRadius
    {
        get;
        private set;
    }
    public int BuildCost
    {
        get;
        private set;
    }

    public bool IsCollision
    {
        get
        {
            return hitColliders.Count > 0 ? true : false;
        }
    }

    public bool IsBuild
    {
        get;
        private set;
    }
    
    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        hitCollider = GetComponent<Collider>();

        CheckIfTurret();
    }

    private void OnEnable()
    {
        Rb.isKinematic = false;
        IsBuild = false;
    }

    private void OnDisable()
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
        CheckIfBuilt();

        Rb.isKinematic = true;
        LastPlacementPosition = transform.position;

        // Debug.Log("SuccessfulPlacement");
    }

    public void ReplaceObject()
    {
        Rb.isKinematic = true;
        transform.position = LastPlacementPosition;
    }

    private void CheckIfTurret()
    {
        var baseTurret = GetComponent<BaseTurret>();
        if (baseTurret != null)
        {
            this.baseTurret = baseTurret;
            ViewRadius = this.baseTurret.ViewRadius;
            BuildCost = this.baseTurret.BuildCost;
        }
    }

    private void CheckIfBuilt()
    {
        if (!IsBuild)
        {
            IsBuild = true;
            PlayerStats.Instance.AddEnergy(-BuildCost);        
        }
    }
}
