using UnityEngine;
using System.Collections;

public class JumpToCliffState : IState
{
	private Vector3 targetPosition = Vector3.zero;
	private Lemming lemming;
	public JumpToCliffState (Lemming lemming)
	{
		targetPosition = lemming.randomPositionOfCurrentTargetPosition.Value;
		this.lemming = lemming;
	}

	public void Update ()
	{
		float step = 3f * Time.deltaTime;
		lemming.transform.position = Vector3.MoveTowards(lemming.transform.position, targetPosition, step);

		if (Vector3.Magnitude (lemming.transform.position - targetPosition) < 0.05f)
		{
			CreateWaveEffect();
			lemming.gameObject.SetActive(false);
		}
	}

	private string splooshPrefabPath = "Unit/Lemming/SplooshPrefab";
	private void CreateWaveEffect()
	{
		var splooshPrefab = (GameObject)Resources.Load(splooshPrefabPath, typeof(GameObject));
		var sploosh = GameObject.Instantiate (splooshPrefab, lemming.gameObject.transform.position, Quaternion.identity) as GameObject;
		sploosh.transform.SetParent (GameController.Instance.map.gameObject.transform);
	}
}
