using UnityEngine;

public class Missile : BaseProjectile
{
    private string hitEffect = "BigExplosionEffect";
    private ParticleSystem missileTrailEffect;
    private float explosionRadius = 10f;

    private Missile()
    {
        damage = 400f;
        projectileSpeed = 40f;
    }

    protected override void Awake()
    {
        base.Awake();
        missileTrailEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        if (missileTrailEffect.isPlaying == false)
            missileTrailEffect.Play();
    }

    private void OnDisable()
    {
        missileTrailEffect.Stop();
    }

    protected override void HitTarget()
    {
        ObjectPoolManager.Instance.GetObjectFromPool(hitEffect, transform.position, Quaternion.identity);

        var unitsInHitRadius = Physics.OverlapSphere(transform.position, explosionRadius, hitLayer);

        foreach (var unit in unitsInHitRadius)
        {
            Unit hitTarget = unit.transform.GetComponent<Unit>();
            if(hitTarget != null)
            {
                hitTarget.TakeDamage(damage);
            }
        }     

        base.HitTarget();
    }
}
