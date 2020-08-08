using UnityEngine;

public class MachineGunTurret : BaseTurret
{
    private GameObject machineGunEffect;
    private GameObject muzzleEffects;
    private ParticleSystem gunImpactEffect;

    private readonly float machineGunDamage = 40f;

    private MachineGunTurret()
    {
        turrentAttackRadius = 10f;
        rotationSpeed = 200f;
    }

    protected override void Awake()
    {
        base.Awake();
        machineGunEffect = firePoint.Find("MachineGunImpactEffect").gameObject;
        gunImpactEffect = machineGunEffect.GetComponentInChildren<ParticleSystem>();
        muzzleEffects = firePoint.transform.Find("MuzzleEffects").gameObject;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StopAttack();
    }

    protected override void Attack()
    {
        FireMachineGun();

        if (!IsCurrentTargetValid)
            StopAttack();
    }

    private void FireMachineGun()
    {
        if(machineGunEffect.activeSelf == false)
        {
            machineGunEffect.SetActive(true);
            muzzleEffects.SetActive(true);
            gunImpactEffect.Play();
        }  

        CurrentTarget.TakeDamage(machineGunDamage * Time.deltaTime);

        Vector3 direction = firePoint.position - CurrentTarget.transform.position;
        gunImpactEffect.transform.position = CurrentTarget.transform.position + direction.normalized * 0.5f;
        gunImpactEffect.transform.rotation = Quaternion.LookRotation(direction);
    }

    protected override void StopAttack()
    {
        gunImpactEffect.Stop();
        machineGunEffect.SetActive(false);
        muzzleEffects.SetActive(false);
    }
}

