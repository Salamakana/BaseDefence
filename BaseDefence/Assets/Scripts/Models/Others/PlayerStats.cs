public class PlayerStats : Singelton<PlayerStats>
{
    public float MaxEnergy
    {
        get;
        set;
    }

    public float CurrentEnergy
    {
        get;
        private set;
    }

    private readonly float startEnergy = 2000f;

    private void Start()
    {
        MaxEnergy = startEnergy;
        AddEnergy(startEnergy);
    }

    public void AddEnergy(float energyReward)
    {
        CurrentEnergy += energyReward;
        UIManager.Instance.UpdateUI();
    }
}