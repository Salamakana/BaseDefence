using UnityEngine.UI;

public class GameInfoPanel : BasePanel
{
    private Image energyBarFill;
    private Text energyText;
    private Text levelStateText;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        energyBarFill = transform.Find("EnergyBar").transform.Find("Fill").GetComponent<Image>();
        energyText = transform.Find("EnergyBar").transform.Find("EnergyText").GetComponent<Text>();
        levelStateText = transform.transform.Find("LevelStateText").GetComponent<Text>();
    }

    private void UpdateGameInfoPanel()
    {
        energyText.text = "ENERGY: " + PlayerStats.Instance.CurrentEnergy;
        energyBarFill.fillAmount = PlayerStats.Instance.CurrentEnergy / PlayerStats.Instance.MaxEnergy;
        levelStateText.text = "LEVEL STATE: " + LevelManager.Instance.LevelState.ToString().ToUpper();
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();
        UpdateGameInfoPanel();
    }

    public void OnGameModeButtonClicked()
    {
        LevelManager.Instance.IsSpawning();
        UpdateGameInfoPanel();
    }

    public void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}
