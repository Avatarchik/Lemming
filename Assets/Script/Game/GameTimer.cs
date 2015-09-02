using UnityEngine;
using UnityEngine.UI;
using System;

public class GameTimer : MonoBehaviour
{
	[SerializeField]
	private Text
		timeText;
	private float currentTime = 0;
	private bool timerStarted = false;
    
	void FixedUpdate ()
	{
		if (timerStarted) {
			currentTime += Time.deltaTime;
			SetTimeText (currentTime);
			CheckTicker();
		}
	}

	public void StartTimer ()
	{
		timerStarted = true;
	}
    
	public void StopTimer ()
	{
		timerStarted = false;
	}

	public float GetCurrentTime()
	{
		return currentTime;
	}
    
	public void ResetTime ()
	{
		currentTime = 0;
		SetTimeText (currentTime);
		ResetTicker ();
	}

	private void SetTimeText (float sec)
	{
		TimeSpan time = TimeSpan.FromSeconds (sec);
		timeText.text = string.Format ("{0:D2}:{1:D2}:{2:D2}", time.Minutes, time.Seconds, time.Milliseconds / 10);
	}

	private float tickTime = 0f;
	private float nextCallTime = 0f;
	private Action tickBehaiver = null;

	private void ResetTicker ()
	{
		nextCallTime = 0f;
		tickTime = 0f;
		tickBehaiver = null;
	}

	private void CheckTicker ()
	{
		if (tickBehaiver != null && currentTime != 0 && nextCallTime < currentTime)
		{
			tickBehaiver();
			nextCallTime += tickTime;
		}
	}

	public void RegisterTicker(float tickTime, Action behaivor)
	{
		this.tickTime = tickTime;
		this.tickBehaiver = behaivor;
		this.nextCallTime = tickTime;
	}
}