using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    protected LayerMask hitLayer;
    protected Unit targetEnemy;
    protected float projectileSpeed;
    protected float damage;

    protected virtual void Awake()
    {
        hitLayer = LayerMask.GetMask("Target");
    }

    public void Launch(Unit targetEnemy)
    {
        this.targetEnemy = targetEnemy;
    }

    private void Update()
    {
        if(targetEnemy == null || targetEnemy.IsActive == false)
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
            return;
        }

        Vector3 direction = targetEnemy.transform.position - transform.position;
        float distanceInThisFrame = projectileSpeed * Time.deltaTime;

        if(direction.magnitude <= distanceInThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceInThisFrame, Space.World);
        transform.LookAt(targetEnemy.transform);
    }

    protected virtual void HitTarget()
    {
        // Animation ? ...

        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
    }
}
