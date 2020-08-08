using UnityEngine;

public class SelectableRadius : MonoBehaviour
{
    private const float OFFSET_FROM_GROUND = 0.2f;
    private Vector3 selectedPosition = new Vector3(0, OFFSET_FROM_GROUND, 0);
    private SpriteRenderer spriteRenderer;

    private Color validPlacementColor = new Color(0, 0, 1, 0.5f);
    private Color unvalidPlacementColor = new Color(1, 0, 0, 0.5f);
    private Color selectedColor = new Color(0, 1, 0, 0.25f);

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ShowViewRadius(Transform selectedObject, float viewRadius)
    {
        viewRadius *= 2;
        transform.localScale = new Vector3(viewRadius, viewRadius, 1);
        transform.SetParent(selectedObject);
        transform.localPosition = selectedPosition;
        gameObject.SetActive(true);
    }

    public bool ShowDoWeHaveValidPositionInColor(bool isValidPosition)
    {
        spriteRenderer.color = isValidPosition ? validPlacementColor : unvalidPlacementColor;
        return isValidPosition;
    }

    public void Show()
    {
        spriteRenderer.color = selectedColor;
    }

    public void DisableViewRadius()
    { 
        transform.SetParent(null);
        transform.localPosition = selectedPosition;
        gameObject.SetActive(false);
    }
}
