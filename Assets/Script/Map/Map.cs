using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Map {
	protected readonly List<Vector3> cliffPosition;
	protected readonly Vector3 centerPosition;

	public Map() {

	}

	public Vector3 GetCenterPosition() {
		return centerPosition;
	}
}
