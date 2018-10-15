using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singelton<InputManager>
{
	#region VARIABLES

	private bool isZooming = false;
	private float cameraPanSpeed = 200f;
	private float mouseScrollSpeed = 2000F;

	private SelectableObject currentlySelectedObject;
	private CameraEngine mainCameraEngine;
	private LayerMask whatIsMoveableObject;
	private LayerMask whatIsGround;
	private Ray moRay;
	private RaycastHit moHit;
	private RaycastHit mouseHitPosition;
	private Vector3 mousePositionRelativeToGround;
	private Vector2 firstTouchPreviousPosition;
	private Vector2 secondTouchPreviousPosition;

	private Touch firstTouch;
	private Touch secondTouch;

	private float touchesPreviousPositionDifference;
	private float touchesCurrentPositionDifference;
	private float zoomModifier;
	private float zoomModifierSpeed = 0.1f;

	#endregion VARIABLES

	#region PROPERTIES

	public SelectableObject SelectableObject
	{
		get
		{
			return currentlySelectedObject;
		}
		set
		{
			currentlySelectedObject = value;
		}
	}
	public ViewRange ViewRange
	{
		get;
		private set;
	}

	public Vector3 MouseHitPoint
	{
		get
		{
			return mouseHitPosition.point;
		}
	}
	public bool DoWeHaveTouches
	{
		get
		{
			return Input.touchCount > 0;
		}
	}
	public bool MoveableObjectGrabbed
	{
		get;
		private set;
	}

	#endregion PROPERTIES

	public bool IsOverUIElement()
	{
#if UNITY_EDITOR
		return EventSystem.current.IsPointerOverGameObject();
#else
		return EventSystem.current.IsPointerOverGameObject(firstTouch.fingerId);
#endif
	}

	private Vector3 UpdatePosition()
	{
		return new Vector3  (
							mousePositionRelativeToGround.x,
							mousePositionRelativeToGround.y,
							mousePositionRelativeToGround.z
							);
	}

	private void Start ()
	{
		Initialize();
	}

	private void Initialize()
	{
		whatIsMoveableObject = LayerMask.GetMask("MoveableObject");
		whatIsGround = LayerMask.GetMask("Terrain");
		mainCameraEngine = Camera.main.GetComponentInParent<CameraEngine>();
		moHit = new RaycastHit();

		ViewRange = ObjectPoolManager.Instance.GetObjectFromPool
			(
			"ViewRange",
			Vector3.zero, 
			Quaternion.Euler(new Vector3(90, 0, 0)), 
			false
			).GetComponent<ViewRange>();
	}

	private void Update()
	{
#if UNITY_EDITOR

		moRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Input.GetMouseButtonDown(0))
		{
			FindAndGrabMoveableObject();
		}   

		if (Input.GetMouseButton(0))
		{
			CheckMoveableObject();         
		}

		if (Input.GetMouseButtonUp(0))
		{
			DropMoveableObject();
		}

		MouseScroll();

#elif UNITY_ANDROID

		moRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

		if (DoWeHaveTouches)
		{
			OnFirstTouch();

			if (Input.touchCount == 2)
			{
				OnSecondTouch();
			}
		}		
#endif
	}

	private void CheckMoveableObject()
	{
		MoveableObjectGrabbed = currentlySelectedObject != null;

		TraceMousePositionRelativeToGround();

		CheckCorrectRangeVisuals();

	}

	private void FindAndGrabMoveableObject()
	{
		if (Physics.Raycast(moRay, out moHit, Mathf.Infinity, whatIsMoveableObject))
		{
			var newSelectableObject = moHit.transform.GetComponent<SelectableObject>();

			if (newSelectableObject != null)
			{
				SelectableObject = newSelectableObject;
				SelectableObject.BeingSelected();
				ViewRange.ShowViewRadius(SelectableObject.transform, SelectableObject.ViewRadius);
			}
		}
		else
		{
			currentlySelectedObject = null;
			ViewRange.ShowDisableViewRadius();
		}
	}

	private void TraceMousePositionRelativeToGround()
	{
		if (Physics.Raycast(moRay, out mouseHitPosition, Mathf.Infinity, whatIsGround))
		{
			if (MoveableObjectGrabbed)
			{
				mousePositionRelativeToGround = mouseHitPosition.point;

				currentlySelectedObject.transform.position = UpdatePosition(/*HitPosition*/);
			}			
			else
			{
				if (!isZooming && !IsOverUIElement())
				{
					PanCamera();
				}					
			}
		}
	}

	private void DropMoveableObject()
	{
		if (currentlySelectedObject != null && !IsOverUIElement() && !currentlySelectedObject.IsCollision)
		{
			 // Debug.LogWarning("SuccessfulPlacement");
			currentlySelectedObject.SuccessfulPlacement();
		}
		else
		{
			if (currentlySelectedObject != null && currentlySelectedObject.IsCollision && SelectableObject.IsBuild)
			{
				// Debug.LogWarning("ReplaceObject");
				currentlySelectedObject.ReplaceObject();
			}
			else if (currentlySelectedObject != null && !SelectableObject.IsBuild)
			{
				// Debug.LogWarning("ReturnObjectToPool");
				ObjectPoolManager.Instance.ReturnObjectToPool(currentlySelectedObject.gameObject);
			}
			else
			{
				if(SelectableObject != null)
				currentlySelectedObject.ReplaceObject();
			}
		}

		// Reset values after we do not have control over an object
		ViewRange.ShowUnvalidPlacementColor(Color.blue);
		// HitPosition = Vector3.zero;
		MoveableObjectGrabbed = false;
	}

	

	public void CheckCorrectRangeVisuals()
	{
		if(currentlySelectedObject != null)
		{
			if (IsOverUIElement() || currentlySelectedObject.IsCollision)
			{
				ViewRange.ShowUnvalidPlacementColor(Color.red);
			}
			else
			{
				ViewRange.ShowUnvalidPlacementColor(Color.blue);
			}
		}		
	}

#if UNITY_ANDROID

	private void OnFirstTouch()
	{
		firstTouch = Input.GetTouch(0);

		switch (firstTouch.phase)
		{
			case TouchPhase.Began:

				FindAndGrabMoveableObject();

				break;

			case TouchPhase.Moved:

				CheckMoveableObject();

				break;

			case TouchPhase.Ended:

				isZooming = false;

				DropMoveableObject();

				break;		
		}
	}

	private void OnSecondTouch()
	{       
		secondTouch = Input.GetTouch(1);

		switch (secondTouch.phase)
		{
			case TouchPhase.Began:

				isZooming = true;

				break;

			case TouchPhase.Moved:

				PinchZoom();

				break;

			case TouchPhase.Ended:

				isZooming = false;

				break;
		}
	}

	public void PinchZoom()
	{
		firstTouchPreviousPosition = firstTouch.position - firstTouch.deltaPosition;
		secondTouchPreviousPosition = secondTouch.position - secondTouch.deltaPosition;

		touchesPreviousPositionDifference = (firstTouchPreviousPosition - secondTouchPreviousPosition).magnitude;
		touchesCurrentPositionDifference = (firstTouch.position - secondTouch.position).magnitude;

		zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

		if (touchesPreviousPositionDifference > touchesCurrentPositionDifference)
		{
			mainCameraEngine.ZoomOrthographicSize(zoomModifier);
		}
		else
		{
			mainCameraEngine.ZoomOrthographicSize(-zoomModifier);
		}	
	}

#endif

	private void PanCamera()
	{
		float cameraX = 0f;
		float cameraY = 0f;

#if UNITY_EDITOR

		cameraX = Input.GetAxis("Mouse X") * cameraPanSpeed * Time.deltaTime;
		cameraY = Input.GetAxis("Mouse Y") * cameraPanSpeed * Time.deltaTime;


#elif UNITY_ANDROID

		Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
		cameraX -= touchDeltaPosition.x * cameraPanSpeed / 2 * Time.deltaTime;
		cameraY -= touchDeltaPosition.y * cameraPanSpeed / 2 * Time.deltaTime;	
		
#endif

		mainCameraEngine.MoveCamera(new Vector2(cameraX,cameraY));
	}

	private void MouseScroll()
	{
		var scroll = Input.GetAxis("Mouse ScrollWheel");
		mainCameraEngine.ZoomOrthographicSize(-scroll * mouseScrollSpeed * Time.deltaTime);
	}
}
