using UnityEngine;
using System.Collections;
using System.Linq;

public class ItemBox : MonoBehaviour {
	[SerializeField]
	private GameObject[] itemImages;
	private ItemController.ItemType? attainedItem = null;

	public bool HasItem()
	{
		return attainedItem.HasValue;
	}

	public void ResetBox()
	{
		attainedItem = null;
		HideItemImages ();
	}

	private void HideItemImages()
	{
		itemImages.ToList ().ForEach (itemImage => itemImage.SetActive (false));
	}

	[UnityEventListener]
	public void UseItem()
	{
		if (!attainedItem.HasValue)
			return;

		switch (attainedItem.Value) {
		case ItemController.ItemType.BrokenClock:
		case ItemController.ItemType.Bush:
		case ItemController.ItemType.Hole:
		case ItemController.ItemType.Mud:
		case ItemController.ItemType.MushRoom:
		case ItemController.ItemType.Thunder:
		case ItemController.ItemType.Wolf:
			ThunderEffect.CreateEffect();
			break;
		}

		ResetBox ();
	}

	private void ShowProperItemImages(ItemController.ItemType itemType)
	{
		itemImages [(int)itemType].SetActive (true);
	}

	public void AttainItem(ItemController.ItemType itemType)
	{
		ShowProperItemImages (itemType);
		attainedItem = itemType;
	}
}
