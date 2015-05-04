using UnityEngine;
using System.Collections;
using ConditionalAttribute = System.Diagnostics.ConditionalAttribute;
using System;

public class LemmingInputController : MonoBehaviour
{
	public delegate void InputEventHandler (Vector2 dragDir);

	private event InputEventHandler inputEventHandler;

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
		var touchCount = Input.touchCount;
		if (Input.touchCount == 0)
			return;

		for (var i = 0; i < touchCount; i++) {
			var touch = Input.GetTouch (i);
			inputEventHandler (touch.deltaPosition.normalized);
		}
	}

	private Vector2? previousMousePosition = null;
	[Conditional ("UNITY_EDITOR")]
	private void CheckMouseInput ()
	{
		const int LEFT_BUTTON_OF_MOUSE = 0;
		if (Input.GetMouseButton (LEFT_BUTTON_OF_MOUSE)){
			if (previousMousePosition.HasValue) {
				var deltaMousePosition = previousMousePosition.Value - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				inputEventHandler (deltaMousePosition.normalized);
			}
			previousMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		}
	}

	public void RegisterInputEventHandler (InputEventHandler callback)
	{
		inputEventHandler += callback;
	}
}
