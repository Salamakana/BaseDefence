using UnityEngine;

public class FlameTurret : BaseTurret
{
    private ParticleSystem flamerEffect;

    private readonly float flamerDamage = 25f;

    private FlameTurret()
    {
        turrentAttackRadius = 10f;
        rotationSpeed = 400f;
    }

    protected override void Awake()
    {
        base.Awake();

        flamerEffect = firePoint.Find("FlameEffect").GetComponent<ParticleSystem>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StopAttack();
    }

    protected override void Attack()
    {
        FireFlamer();

        if (!IsCurrentTargetValid)
            StopAttack();
    }

    private void FireFlamer()
    {
        if (flamerEffect.isPlaying == false)
        {
            flamerEffect.Play();
        }

        CurrentTarget.TakeDamage(flamerDamage * Time.deltaTime);
    }

    protected override void StopAttack()
    {
        flamerEffect.Stop();
    }
}

