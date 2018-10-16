using UnityEngine;


public class UIManager : Singelton<UIManager>
{
    public Transform HUDCanvas
    {
        get;
        private set;
    }
    public BuildPanel BuildPanel
    {
        get;
        private set;
    }
    public GameInfoPanel GameInfoPanel
    {
        get;
        private set;
    }
    public SelectedInfoPanel SelectedInfoPanel
    {
        get;
        private set;
    }

    private void Awake()
    {
        HUDCanvas = transform.Find("HUDCanvas");
        BuildPanel = HUDCanvas.Find("BuildPanel").GetComponent<BuildPanel>();
        GameInfoPanel = HUDCanvas.transform.Find("GameInfoPanel").GetComponent<GameInfoPanel>();
        SelectedInfoPanel = HUDCanvas.transform.Find("SelectedInfoPanel").GetComponent<SelectedInfoPanel>();
    }

    private void Start()
    {
        HUDCanvas.gameObject.SetActive(true);
        BuildPanel.gameObject.SetActive(true);
        GameInfoPanel.gameObject.SetActive(true);
        SelectedInfoPanel.gameObject.SetActive(false);
    }

    public void UpdateUI()
    {
        BuildPanel.UpdatePanel();
        GameInfoPanel.UpdatePanel();
    }

    public void UpdateSelectedObjectInfo(CastableButton selectedCastableButton, CastableBlueprint castableBlueprint)
    {
        SelectedInfoPanel.gameObject.SetActive(true);
        InputManager.Instance.CurrentCastableButtonSelected = selectedCastableButton;
        SelectedInfoPanel.SetSelectedInfoPanel(castableBlueprint);
    }
}
