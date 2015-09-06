using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameOverPopup : MonoBehaviour
{
	[SerializeField]
	private Text
		currentScoreText;
	[SerializeField]
	private Text
		bestScoreText;
	[SerializeField]
	private GameObject
		newLabel;

	public void Initialize (float currentScore)
	{
		TimeSpan currentTime = TimeSpan.FromSeconds (currentScore);
		currentScoreText.text = string.Format ("{0:D2}:{1:D2}:{2:D2}", currentTime.Minutes, currentTime.Seconds, currentTime.Milliseconds / 10);

		if (currentScore > User.GetInstance.Record) {
			bestScoreText.text = string.Format ("{0:D2}:{1:D2}:{2:D2}", currentTime.Minutes, currentTime.Seconds, currentTime.Milliseconds / 10);
			newLabel.SetActive (true);
			LemmingNetwork.GetInstance.SetRecord (currentScore, (r) => {
				User.GetInstance.Record = currentScore;
				Debug.Log ("record saved");
			}, (r) => {
				Debug.LogError ("saving record error");
			});
		} else {
			TimeSpan bestTime = TimeSpan.FromSeconds(User.GetInstance.Record);
			bestScoreText.text = string.Format ("{0:D2}:{1:D2}:{2:D2}", bestTime.Minutes, bestTime.Seconds, bestTime.Milliseconds / 10);
			newLabel.SetActive(false);
		}
	}
}
