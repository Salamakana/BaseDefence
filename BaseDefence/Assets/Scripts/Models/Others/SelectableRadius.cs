using UnityEngine;

public class SelectableRadius : MonoBehaviour
{
    private const float OFFSET_FROM_GROUND = 0.2f;
    private Vector3 selectedPosition = new Vector3(0, OFFSET_FROM_GROUND, 0);
    private SpriteRenderer spriteRenderer;

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

    public void ShowUnvalidPlacementColor(Color newColor)
    {
        if (spriteRenderer.color.Equals(newColor))
            return;

        spriteRenderer.color = newColor;
    }

    public void ShowDisableViewRadius()
    { 
        transform.SetParent(null);
        transform.localPosition = selectedPosition;
        gameObject.SetActive(false);
    }
}
