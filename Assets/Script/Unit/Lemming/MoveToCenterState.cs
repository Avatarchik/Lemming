using UnityEngine;
using System.Collections;

public class BackToCenterState : IState
{
	private Vector2 centerPosition;
	private Lemming lemming;

	public BackToCenterState (Lemming lemming)
	{
		this.lemming = lemming;
		centerPosition = GameController.Instance.GetCenterPosition();
	}

	private bool IsReached ()
	{
		const float allowanceValue = 0.1f;
		return GetDistance (lemming.transform.position, centerPosition) < allowanceValue;
	}

	private float GetDistance (Vector2 a, Vector2 b)
	{
		return (a - b).magnitude;
	}

	public void Update ()
	{
		if (IsReached ())
			InitializeLemmingState ();
		else
			MoveToCenter ();
	}

	private void MoveToCenter ()
	{
		float step = lemming.speed * Time.deltaTime;
		lemming.transform.position = Vector3.MoveTowards (lemming.transform.position, centerPosition, step);
	}

	private void InitializeLemmingState ()
	{
		lemming.transform.position = centerPosition;
		lemming.ChangeAction (Lemming.Action.ReadyToRunInCenter);
	}
}
