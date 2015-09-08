﻿using UnityEngine;
using ConditionalAttribute = System.Diagnostics.ConditionalAttribute;
using System.Collections.Generic;
using System.Linq;

public class LemmingInputController : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        CheckTouchInput();
        CheckMouseInput();
    }

    private void CheckTouchInput()
    {
        if (GameController.Instance.CurrentGameState != GameController.GameState.Start)
            return;

        var touchCount = Input.touchCount;
        if (touchCount == 0)
            return;

        for (var i = 0; i < touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            TriggerTouchEvent(touch.position, touch.deltaPosition);
        }
    }

    private void TriggerTouchEvent(Vector2 position, Vector2 deltaPosition)
    {
        if (deltaPosition == Vector2.zero)
            return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(position);
		Collider2D[] hitColliders = Physics2D.OverlapPointAll(pos);

		hitColliders.ToList ().ForEach (hitCollider => {
			if (hitCollider != null && hitCollider.gameObject.name.Contains ("TouchTrigger"))
				GameController.Instance.TouchInputTrigger (hitCollider.gameObject);
		});
    }

    [Conditional("UNITY_EDITOR")]
    private void CheckMouseInput()
    {
        if (GameController.Instance.CurrentGameState != GameController.GameState.Start)
            return;

        const int LEFT_BUTTON_OF_MOUSE = 0;
        if (Input.GetMouseButton(LEFT_BUTTON_OF_MOUSE))
        {
            // FIXME : Not Implemented yet.
            TriggerTouchEvent(Input.mousePosition, new Vector2(0, -10));
        }
    }
}
