using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FindAvailableCliffState : IState
{
	bool isCalled = false;
    private Lemming lemming;
	public FindAvailableCliffState (Lemming lemming)
	{
        this.lemming = lemming;
	}

	public void Update ()
	{
		// FIXME : This will be called after finding animation.
		if (isCalled)
			return;

		GameController.Instance.BroadcastToFindNewTargetToAllLemmings (GetRandomTargetPosition ());
        lemming.ChangeAction(Lemming.Action.MoveToCliff);
		isCalled = true;
	}

	private HexagonMap.MapPosition GetRandomTargetPosition ()
	{
		return GameController.Instance.map.GetRandomMapPosition();
	}
}
