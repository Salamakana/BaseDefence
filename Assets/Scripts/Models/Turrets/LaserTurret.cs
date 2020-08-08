using UnityEngine;

public class LaserTurret : BaseTurret
{
    private LineRenderer lineRenderer;
    private GameObject laserImpactEffect;
    private Light hitLight;

    private float laserDamage = 150f;

    private LaserTurret()
    {
        turrentAttackRadius = 10f;
        rotationSpeed = 150f;       
    }

    protected override void Awake()
    {
        base.Awake();

        lineRenderer = GetComponentInChildren<LineRenderer>();
        laserImpactEffect = lineRenderer.transform.Find("LaserImpactEffect").gameObject;
        hitLight = GetComponentInChildren<Light>(); 
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StopAttack();
    }

    protected override void Attack()
    {
        ShootLaser();

        if (!IsCurrentTargetValid)
            StopAttack();
    }

    private void ShootLaser()
    {
        if (laserImpactEffect.activeSelf == false)
        {
            laserImpactEffect.SetActive(true);
            lineRenderer.enabled = true;
            hitLight.enabled = true; 
        }

        CurrentTarget.TakeDamage(laserDamage * Time.deltaTime);

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, CurrentTarget.transform.position);

        Vector3 direction = firePoint.position - CurrentTarget.transform.position;
        laserImpactEffect.transform.position = CurrentTarget.transform.position + direction.normalized * 0.5f;
        laserImpactEffect.transform.rotation = Quaternion.LookRotation(direction);
    }

    protected override void StopAttack()
    {
        lineRenderer.enabled = false;
        hitLight.enabled = false;
        laserImpactEffect.SetActive(false);
    }
}
