using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lemming : MonoBehaviour{
	private float speed;
	private float power;

	void Awake() {
		// FIXME: read speed and power information from table.
		speed = 0.5f;
		power = 0.5f;
	}
}
