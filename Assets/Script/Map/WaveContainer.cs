using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class WaveContainer : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> waves;
	private GameObject wavePrefab;

	void Awake()
	{
		var prefabPath = "Background/smallWave";
		wavePrefab = (GameObject)Resources.Load(prefabPath, typeof(GameObject));
	}

	void Start()
	{
		StartCoroutine (CreateSmallWavesJob());
	}

	private IEnumerator CreateSmallWavesJob()
	{
		const int maximumTickTime = 5;
		const float maximumWaveCount = 5;

		while(true)
		{
			var waveCount = UnityEngine.Random.Range (0, maximumWaveCount);

			for (var i = 0 ; i < waveCount; i++)
			{
				var randomWave = waves.ElementAt( UnityEngine.Random.Range(0, waves.Count()));
				CreateSmallWave(randomWave);
			}

			var nextWaveCreateTime = UnityEngine.Random.Range(0, maximumTickTime);
			yield return new WaitForSeconds(nextWaveCreateTime);
		}
	}

	private void CreateSmallWave(GameObject parentWave)
	{
		var wave = GameObject.Instantiate (wavePrefab) as GameObject;
		wave.transform.parent = parentWave.transform;
		var waveWidth = parentWave.GetComponent<RectTransform> ().sizeDelta.x;

		var creatingXPosition = UnityEngine.Random.Range (0, waveWidth / 2);
		var multyNum = UnityEngine.Random.Range (0, 1) == 1 ? 1 : -1;

		wave.GetComponent<RectTransform>().localPosition = new Vector3(creatingXPosition * multyNum, 0, 0);
		wave.GetComponent<RectTransform>().localScale = Vector3.one;
	}
}
