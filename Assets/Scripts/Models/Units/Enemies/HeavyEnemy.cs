public class HeavyEnemy : Unit
{
    protected override void Awake()
    {
        baseSpeed = 3.5f;
        baseAcceleration = 4f;
        deathEffect = "SmallExplosionEffect";

        rewardEnergy = 50;

        base.Awake();
    }

    protected override void OnEnable()
    {
        health = 200f;
        base.OnEnable();
    }
}
