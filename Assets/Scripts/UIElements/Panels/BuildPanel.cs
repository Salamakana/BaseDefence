using UnityEngine;

public class BuildPanel : BasePanel
{
    private CastableButton[] castableButtons;

    protected override void Awake()
    {
        base.Awake();

        desiredPosition = transform.position + new Vector3(0, -100);

        castableButtons = GetComponentsInChildren<CastableButton>();
    }

    public void CreatePrefabInstance(string prefabName)
    {
        ObjectPoolManager.Instance.GetObjectFromPool
        (prefabName,
        InputManager.Instance.MouseHitPoint,
        Quaternion.identity
        );
    } 

    private void UpdateCastableButtonStates()
    {
        foreach (var castableButton in castableButtons)
        {
            var currentEnergy = PlayerStats.Instance.CurrentEnergy;
            castableButton.SetActiveState((currentEnergy -= castableButton.CastableBlueprint.EnergyCost) < 0 ? false : true);
        }
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();
        UpdateCastableButtonStates();
    }
}
