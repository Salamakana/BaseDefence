using UnityEngine;

public class CameraEngine : MonoBehaviour
{
	private const float MIN_ORTHOGRAPHIC_SIZE = 10f;
	private const float MAX_ORTHOGRAPHIC_SIZE = 30f;

	private new Camera camera;

	private void Awake()
	{
		camera = GetComponent<Camera>();
	}

	public Vector3 MainCameraPosition()
	{
		return transform.position;
	}

	public void ZoomOrthographicSize(float zoomModifier)
	{
		camera.orthographicSize += zoomModifier;

		camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, MIN_ORTHOGRAPHIC_SIZE, MAX_ORTHOGRAPHIC_SIZE);
	}

	public void MoveCamera(Vector2 newPosition)
	{
		var newPositionX = newPosition.x;
		var newPositionY = newPosition.y;

		transform.position -= new Vector3(newPositionX, 0, newPositionY);		
	}
}
