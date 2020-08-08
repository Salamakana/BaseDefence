public class PlayerStats : Singelton<PlayerStats>
{
    public float MaxEnergy
    {
        get;
        private set;
    }
    public float CurrentEnergy
    {
        get;
        private set;
    }
    private readonly float startEnergy = 4000f;

    private void Start()
    {
        MaxEnergy = startEnergy;
        AddEnergy(startEnergy);
    }

    public void AddEnergy(float energyAmount)
    {
        CurrentEnergy += energyAmount;
        UIManager.Instance.UpdateUI();
    }
}