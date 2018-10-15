using UnityEngine;

public class MissileTurret : BaseTurret
{
    private readonly string projectilePrefabName = "Missile";

    private MissileTurret()
    {
        fireRate = 0.5f;
        fireCountdown = 1f;

        turrentAttackRadius = 25f;
        rotationSpeed = 80f;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Attack()
    {
        if(fireCountdown <= 0f)
        {
            var projectile = ObjectPoolManager.Instance.GetObjectFromPool(projectilePrefabName, firePoint.transform.position, Quaternion.identity).GetComponent<BaseProjectile>();
            projectile.Launch(CurrentTarget);

            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    protected override void StopAttack()
    {

    }
}
