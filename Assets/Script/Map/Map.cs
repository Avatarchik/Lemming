using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Map
{
	protected List<Vector2> cliffPosition;
	protected Vector2 centerPosition;

	public Vector2 GetCenterPosition ()
	{
		return centerPosition;
	}

	public List<Vector2> GetCliffPosition ()
	{
		return cliffPosition;
	}
}
