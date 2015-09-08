using UnityEngine;
using System.Collections;

public class DestoryAfterSecond : MonoBehaviour
{

	[SerializeField]
	private float
		seconds = 0f;

	void Start ()
	{
		StartCoroutine (DestroyJob ());
	}

	private IEnumerator DestroyJob ()
	{
		yield return new WaitForSeconds (seconds);
		GameObject.Destroy (gameObject);
	}
}
