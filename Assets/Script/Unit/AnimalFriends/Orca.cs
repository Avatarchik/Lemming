using UnityEngine;
using System.Collections;

public class Orca : AnimalFriend
{
	private static string prefabPath = "Unit/AnimalFriends/Orca/OrcaPrefab";
	
	protected override void AnimalMovement() {}

	public static void SpawnAnimal()
	{
		var orcaPrefab = (GameObject)Resources.Load(prefabPath, typeof(GameObject));
		var orca = GameObject.Instantiate (orcaPrefab, GameController.Instance.map.orcaSpawnPoint.position, Quaternion.identity) as GameObject;
		orca.transform.SetParent (GameController.Instance.map.gameObject.transform);
	}
}
