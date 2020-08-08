public class LightEnemy : Unit
{
    protected override void Awake()
    {       
        baseSpeed = 6.5f;
        baseAcceleration = 8f;
        deathEffect = "SmallExplosionEffect";

        rewardEnergy = 20;

        base.Awake();
    }

    protected override void OnEnable()
    {
        health = 100f;
        base.OnEnable();
    }
}
