using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class ItemController : MonoBehaviour {
	private static ItemController itemController;
	[SerializeField]
	private List<ItemBox> itemBoxes;

	public enum ItemType {
		Bush = 0,
		BrokenClock,
		Hole,
		Mud,
		MushRoom,
		Thunder,
		Wolf
	}

	void Awake()
	{
		itemController = this;
	}

	public void ResetItemBoxes()
	{
		itemBoxes.ForEach (itemBox => itemBox.ResetBox ());
	}

	public void AttainRandomItem()
	{
		var countOfItemType = Enum.GetNames (typeof(ItemType)).Length;
		var randomItem = (ItemType)UnityEngine.Random.Range (0, countOfItemType);

		var emptyItemBox = itemBoxes.FirstOrDefault (itemBox => !itemBox.HasItem ());

		if (emptyItemBox != null)
			emptyItemBox.AttainItem(randomItem);
	}

	public static ItemController Instance
	{
		get {
			if (itemController == null)
				itemController = new ItemController();
			return itemController;
		}
	}
}
