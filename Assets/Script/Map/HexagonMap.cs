using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HexagonMap : Map {
	public HexagonMap() {
		cliffPosition = new List<Vector3> { new Vector3(10, 10, 10), new Vector3(20, 20, 20), new Vector3(30, 30, 30) };
		centerPosition = new Vector3 (0, 0, 0);
	}
}
