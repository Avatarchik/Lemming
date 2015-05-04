using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class MoveToCliffState : IState
{
	private Vector2 targetPosition;
	private Lemming lemming;
	private float randomSpeed;

	public MoveToCliffState (Lemming lemming)
	{
		this.lemming = lemming;
		SetRandomSpeed (lemming.speed);
		targetPosition = GetRandomTargetPosition (lemming.AvailableCliffPositionList);
	}

	private void SetRandomSpeed (float speed)
	{
		const float RANDOM_PERCENTAGE_LIMIT = 30;
		var maximumSpeed = speed * (1 + RANDOM_PERCENTAGE_LIMIT / 100);
		var minimumSpeed = speed * (1 - RANDOM_PERCENTAGE_LIMIT / 100);
		this.randomSpeed = Random.Range (minimumSpeed, maximumSpeed);
	}

	public void Update ()
	{
		if (IsValidTargetPosition ())
			MoveToTargetCliff ();
		else
			MoveToCenter ();
	}

	private void MoveToCenter ()
	{
		lemming.ChangeAction (Lemming.Action.BackToCenter);
	}

	private bool IsValidTargetPosition ()
	{
		return lemming.AvailableCliffPositionList.Contains (targetPosition);
	}

	private void MoveToTargetCliff ()
	{
		float step = randomSpeed * Time.deltaTime;
		lemming.transform.position = Vector3.MoveTowards (lemming.transform.position, targetPosition, step);
	}

	private Vector2 GetRandomTargetPosition (List<Vector2> positionList)
	{
		return positionList.Skip (Random.Range (0, positionList.Count)).Take (1).FirstOrDefault ();
	}
}
