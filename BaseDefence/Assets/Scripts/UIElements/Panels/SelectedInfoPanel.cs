using UnityEngine.UI;

public class SelectedInfoPanel : BasePanel
{
    public Text SelectedName
    {
        get;
        private set;
    }
    public Text EnergyCost
    {
        get;
        private set;
    }

    private void Awake()
    {
        SelectedName = transform.Find("SelectedName").GetComponent<Text>();
        EnergyCost = transform.Find("EnergyCost").GetComponent<Text>();
    }

    private void OnEnable()
    {
       
    }

    public void SetSelectedInfoPanel(CastableBlueprint castableBlueprint)
    {
   
        SelectedName.text = castableBlueprint.Name.ToUpper();
        EnergyCost.text = "ENERGY COST: " + castableBlueprint.EnergyCost.ToString().ToUpper();
       
    }

    private void OnDisable()
    {
        
    }
}
