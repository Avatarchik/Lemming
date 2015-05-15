using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FindAvailableCliffState : IState
{
	bool isCalled = false;

	public FindAvailableCliffState (Lemming lemming)
	{
		
	}

	public void Update ()
	{
		// FIXME : This will be called after finding animation.
		if (isCalled)
			return;

		GameController.Instance.BroadcastToFindNewTargetToAllLemmings (GetRandomTargetPosition ());
		isCalled = true;
	}

	private Vector2 GetRandomTargetPosition ()
	{
		var availablePositionList = GameController.Instance.GetAvailableCliffPosition ();
		return availablePositionList.Skip (Random.Range (0, availablePositionList.Count)).Take (1).FirstOrDefault ();
	}
}
