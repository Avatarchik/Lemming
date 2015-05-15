using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchTriggerAction : MonoBehaviour {
	List<Lemming> lemmings = new List<Lemming>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerExit2D(Collider2D collider) {
		Debug.Log("fuckOut");
		var lemming = collider.gameObject.GetComponent<Lemming>();

		if (lemming != null)
			lemmings.Remove(lemming);
	}
	
	void OnTriggerEnter2D(Collider2D collider) {
		Debug.Log("fuck");
		var lemming = collider.gameObject.GetComponent<Lemming>();
		
		if (lemming != null)
			lemmings.Add(lemming);
	}
}
