using UnityEngine;
using System.Collections;

public abstract class AnimalFriend : MonoBehaviour {
	[SerializeField]
	private GameObject PresentObject;

	protected abstract void AnimalMovement ();

	void Update()
	{
		AnimalMovement ();
	}

	public void OnClicked()
	{
		PresentObject.SetActive (false);
		ItemController.Instance.AttainRandomItem ();
	}
}
