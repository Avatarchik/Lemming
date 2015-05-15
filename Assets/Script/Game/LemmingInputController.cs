using UnityEngine;
using ConditionalAttribute = System.Diagnostics.ConditionalAttribute;
using System.Collections.Generic;

public class LemmingTouch {
	private Vector2 beganPosition;
	private Collider2D touchedTrigger;
	public LemmingTouch(Vector2 startPosition) {
		this.beganPosition = startPosition;
	}

	public Collider2D TouchedTrigger{
		get
		{
			return touchedTrigger;
		}
		set
		{
			touchedTrigger = value;
		}
	}
	
	public Vector2 BeganPosition {
		get
		{
			return beganPosition;
		}
	}
}

public class LemmingInputController : MonoBehaviour
{
	private Dictionary<int, LemmingTouch> touchCollection = new Dictionary<int, LemmingTouch>();
	void Start ()
	{
		
	}

	void Update ()
	{
		CheckTouchInput ();
		CheckMouseInput ();
	}

	private void CheckTouchInput ()
	{
		if (GameController.Instance.CurrentGameState != GameController.GameState.Start)
			return;

		var touchCount = Input.touchCount;
		if (Input.touchCount == 0)
			return;

		for (var i = 0; i < touchCount; i++) {
			var touch = Input.GetTouch (i);
			if (touch.phase == TouchPhase.Began)
				TouchBegan(touch.fingerId, touch.position);
			if (touch.phase == TouchPhase.Ended)
				TouchEnded(touch.fingerId, touch.position);
			if (touch.phase == TouchPhase.Moved)
				TouchMoved(touch.fingerId, touch.position);
		}
	}
	
	private void TouchBegan(int fingerID, Vector2 position) {
		if (touchCollection.ContainsKey(fingerID))
			touchCollection.Remove(fingerID);

		touchCollection.Add(fingerID, new LemmingTouch(position));
	}
	
	private void TouchEnded(int fingerID, Vector2 endedPosition) {
		Check (touchCollection[fingerID].BeganPosition, endedPosition, touchCollection[fingerID].TouchedTrigger);
		touchCollection.Remove(fingerID);
	}

	private void TouchMoved(int fingerID, Vector2 position) {
		Vector3 pos = Camera.main.ScreenToWorldPoint (position);
		RaycastHit2D hit = Physics2D.Raycast (pos, Vector2.zero);

		if (hit.collider.gameObject.name.Contains("TouchTrigger")) {
			touchCollection[fingerID].TouchedTrigger = hit.collider;
		}
	}

	private void Check (Vector2 beganPosition, Vector2 endPosition, Collider2D trigger)
	{
		
	}

	[Conditional ("UNITY_EDITOR")]
	private void CheckMouseInput ()
	{
		if (GameController.Instance.CurrentGameState != GameController.GameState.Start)
			return;

		const int LEFT_BUTTON_OF_MOUSE = 0;
		if (Input.GetMouseButton (LEFT_BUTTON_OF_MOUSE)) {

		}
	}
}
