using UnityEngine;

public class BuildPanel : BasePanel
{
    private CastableButton[] castableButtons;

    private void Awake()
    {
        castableButtons = GetComponents<CastableButton>();
    }

    public void CreateTurret(string turretType)
    {
        ObjectPoolManager.Instance.GetObjectFromPool
        (turretType,
        InputManager.Instance.MouseHitPoint,
        Quaternion.identity
        );
    } 

    private void UpdateCastableButtonStates()
    {
        foreach (var castableButton in castableButtons)
        {
            var currentEnergy = PlayerStats.Instance.CurrentEnergy;

            castableButton.SetActiveState((currentEnergy -= castableButton.CastCost) < 0 ? false : true);
        }
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();
        UpdateCastableButtonStates();
    }
}
