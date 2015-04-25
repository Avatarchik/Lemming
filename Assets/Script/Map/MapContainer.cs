using UnityEngine;
using System.Collections;

public class MapContainer : MonoBehaviour {
	public Map map;

	void Awake() {
		// FIXME: Get Map information form global instance.
		map = new HexagonMap ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
