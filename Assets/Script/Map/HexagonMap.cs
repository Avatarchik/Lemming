using UnityEngine;
using System.Collections.Generic;

public class HexagonMap : Map
{
	public HexagonMap ()
	{
		cliffPosition = new List<Vector2> {
			new Vector2 (0, 4.3f),
			new Vector2 (3.6f, 2.4f),
			new Vector2 (3.6f, -2.4f),
			new Vector2 (0, -4.3f),
			new Vector2 (-3.6f, -2.4f),
			new Vector2 (-3.6f, 2.4f)
		};
		centerPosition = new Vector2 (0, 0);
	}
}
