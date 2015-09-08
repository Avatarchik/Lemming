using UnityEngine;
using System.Collections;

public class ThunderEffect : MonoBehaviour
{
	private static string prefabPath = "Item/Prefab/Thunder";

	public static void CreateEffect()
	{
		var thunderPrefab = (GameObject)Resources.Load(prefabPath, typeof(GameObject));
		var thunder = GameObject.Instantiate (thunderPrefab, GameController.Instance.map.thunderSpawnPoint.position, Quaternion.identity) as GameObject;
		thunder.transform.SetParent (GameController.Instance.map.gameObject.transform);
	}

	void Start()
	{
		ApplyEffect ();
	}

	private void ApplyEffect()
	{
		const float lemmingStopTime = 5f;
		GameController.Instance.StunAllLemmingForSecond (lemmingStopTime);
	}
}
