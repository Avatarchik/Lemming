using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RankingPopup : MonoBehaviour
{
	[SerializeField]
	private GameObject
		contentPanel;

	void OnEnable ()
	{
		LemmingNetwork.GetInstance.GetWorldRecords (GetWorldRecordsSuccessCallback, (x) => {});
	}

	private void GetWorldRecordsSuccessCallback (LemmingNetworkResult<GetWorldRecordsResult> result)
	{
		var worldRecords = result.GetFirstResult ();
		RefreshRecord (worldRecords.userRecords);
	}

	private void RefreshRecord (List<UserRecord> records)
	{
		DeleteAllRanking ();
		records.ForEach (record => AttachRankingToPanel (record));
		ResizeAndRepositionPanel (records.Count);
	}

	private void AttachRankingToPanel(UserRecord record)
	{
		var prefabName = string.Empty;
		if (record.rank < 6) {
			prefabName = "Menu/Ranking/rank" + record.rank;
		} else {
			prefabName = "Menu/Ranking/rankOther";
		}
		var prefab = (GameObject)Resources.Load(prefabName, typeof(GameObject));
		var content = GameObject.Instantiate(prefab) as GameObject;
		content.transform.SetParent (contentPanel.transform);
		content.GetComponent<RankingContent> ().Init (record);

		content.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		content.GetComponent<RectTransform> ().localScale = Vector3.one;
	}

	private void ResizeAndRepositionPanel(int countOfRecord)
	{
		const float heightOfRecord = 90f;
		var size = contentPanel.GetComponent<RectTransform> ().sizeDelta;
		contentPanel.GetComponent<RectTransform> ().sizeDelta = new Vector2 (size.x, heightOfRecord * countOfRecord);
	}

	private void DeleteAllRanking ()
	{
		foreach (Transform i in contentPanel.transform) {
			GameObject.Destroy (i.gameObject);
		}
	}
}
