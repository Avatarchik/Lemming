using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	[SerializeField] private MapContainer mapContainer;
	[SerializeField] private Lemming lemming;
	private Map map;

	void Awake () {
		Initialize ();
		ResetLemmingPosition ();
	}

	void Start () {

	}

	void Update () {
	
	}

	private void Initialize(){
		map = mapContainer.map;
	}

	private void ResetLemmingPosition() {
		lemming.transform.position = map.GetCenterPosition ();
	}
}
