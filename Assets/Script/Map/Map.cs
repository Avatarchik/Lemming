using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface Map
{
	Vector2 GetCenterPosition ();

	List<Vector2> GetCliffPosition ();
	void TouchInputTrigger(GameObject trigger);
}
