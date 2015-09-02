using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameOverPopup : MonoBehaviour
{
	[SerializeField]
	private Text currentScoreText;
	[SerializeField]
	private Text bestScoreText;
	[SerializeField]
	private GameObject newLabel;

	public void Initialize(float currentScore)
	{
		TimeSpan time = TimeSpan.FromSeconds (currentScore);
		currentScoreText.text = string.Format ("{0:D2}:{1:D2}:{2:D2}", time.Minutes, time.Seconds, time.Milliseconds / 10);
	}
}
