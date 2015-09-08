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
		if (PresentObject.activeInHierarchy)
		{
			ItemController.Instance.AttainRandomItem ();
			PresentObject.SetActive (false);
		}
	}
}
