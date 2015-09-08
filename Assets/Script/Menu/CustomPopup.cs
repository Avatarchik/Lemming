using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class CustomPopup : MonoBehaviour
{
	[SerializeField]
	private GameObject[] customPanels;

	private void ShowNextPanel()
	{
		var activePanel = customPanels.FirstOrDefault (customPanel => customPanel.activeInHierarchy);

		if (activePanel != null) {
			var activeIndex = Array.IndexOf(customPanels, activePanel);
			var nextActiveIndex = activeIndex == customPanels.Length - 1 ? 0 : activeIndex + 1;
			customPanels.ToList().ForEach(customPanel => customPanel.SetActive(false));
			customPanels[nextActiveIndex].SetActive(true);
		} else {
			var firstPanel = customPanels.First();
			if (firstPanel != null)
				firstPanel.SetActive(true);
		}
	}

	
	[UnityEventListener]
	private void ShowBeforePanel()
	{
		var activePanel = customPanels.FirstOrDefault (customPanel => customPanel.activeInHierarchy);
		
		if (activePanel != null) {
			var activeIndex = Array.IndexOf(customPanels, activePanel);
			var beforeActiveIndex = activeIndex == 0 ? customPanels.Length - 1 : activeIndex - 1;
			customPanels.ToList().ForEach(customPanel => customPanel.SetActive(false));
			customPanels[beforeActiveIndex].SetActive(true);
		} else {
			var firstPanel = customPanels.First();
			if (firstPanel != null)
				firstPanel.SetActive(true);
		}
	}
}
