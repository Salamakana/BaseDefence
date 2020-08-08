using UnityEngine.EventSystems;
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

    protected override void Awake()
    {
        base.Awake();

        SelectedName = transform.Find("SelectedName").GetComponent<Text>();
        EnergyCost = transform.Find("EnergyCost").GetComponent<Text>();
    }

    protected override void Start()
    {
        base.Start();
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

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }
}
