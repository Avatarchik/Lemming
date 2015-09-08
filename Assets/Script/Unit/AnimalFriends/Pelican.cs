using UnityEngine;
using System.Collections;

public class Pelican : AnimalFriend
{
	private static string prefabPath = "Unit/AnimalFriends/Pelican/PelicanPrefab";

	private readonly float pelicanSpeed = 3f;

	protected override void AnimalMovement()
	{
		float step = pelicanSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, GameController.Instance.map.pelicanTargetPoint.position, step);

		if (Vector3.Magnitude (GameController.Instance.map.pelicanTargetPoint.position - transform.position) < 1f)
			GameObject.Destroy (gameObject);
	}

	public static void SpawnAnimal()
	{
		var pelicanPrefab = (GameObject)Resources.Load(prefabPath, typeof(GameObject));
		var pelican = GameObject.Instantiate (pelicanPrefab, GameController.Instance.map.pelicanSpawnPoint.position, Quaternion.identity) as GameObject;
		pelican.transform.SetParent (GameController.Instance.map.gameObject.transform);
	}
}
