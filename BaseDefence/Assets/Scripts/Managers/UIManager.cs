using UnityEngine;


public class UIManager : Singelton<UIManager>
{
    public Transform HUDCanvas
    {
        get;
        private set;
    }
    public BasePanel BuildPanel
    {
        get;
        private set;
    }
    public BasePanel GameInfoPanel
    {
        get;
        private set;
    }

    private void Awake()
    {
        HUDCanvas = transform.Find("HUDCanvas");
        BuildPanel = HUDCanvas.Find("BuildPanel").GetComponent<BasePanel>();
        GameInfoPanel = HUDCanvas.transform.Find("GameInfoPanel").GetComponent< BasePanel>();
    }

    public void UpdateUI()
    {
        BuildPanel.UpdatePanel();
        GameInfoPanel.UpdatePanel();
    }
}
