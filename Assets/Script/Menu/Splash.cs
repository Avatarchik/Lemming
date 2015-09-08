using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour
{
	[SerializeField]
	private GameObject
		popupCanvas;

	void Start ()
	{
		//InactiveAllPopups ();
		gameObject.SetActive (false);
	}

	void Update ()
	{
	
	}

	void InactiveAllPopups ()
	{
		foreach (Transform i in popupCanvas.transform) {
			if (i.gameObject != gameObject)
				i.gameObject.SetActive (false);
		}
	}
}
