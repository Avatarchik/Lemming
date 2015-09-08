using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

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
			CheckTickerAndTimeout();
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
		ResetTimeout ();
	}

	private void SetTimeText (float sec)
	{
		TimeSpan time = TimeSpan.FromSeconds (sec);
		timeText.text = string.Format ("{0:D2}:{1:D2}:{2:D2}", time.Minutes, time.Seconds, time.Milliseconds / 10);
	}

	private List<TickAction> tickActions = new List<TickAction>();
	private List<TimeoutAction> timeoutActions = new List<TimeoutAction>();

	private void ResetTicker ()
	{
		tickActions.Clear ();
	}

	private void ResetTimeout()
	{
		timeoutActions.Clear ();
	}

	private void CheckTickerAndTimeout ()
	{
		if (currentTime == 0)
			return;

		tickActions.ForEach (tickAction => {
			if (tickAction.nextCallTime < currentTime)
			{
				tickAction.behavior();
				tickAction.nextCallTime += tickAction.tickTime;
			}
		});

		timeoutActions.ForEach (timeoutAction => {
			if (timeoutAction.nextCallTime < currentTime)
			{
				timeoutAction.behavior();
				timeoutAction.isCalled = true;
			}
		});

		timeoutActions.RemoveAll (timeoutAction => timeoutAction.isCalled == true);
	}

	public void RegisterTicker(TickAction tickAction)
	{
		tickActions.Add (tickAction);
	}

	public void RemoveTicker(TickAction tickAction)
	{
		tickActions.Remove (tickAction);
	}

	public void RegisterTimeout(TimeoutAction timeout)
	{
		timeout.nextCallTime = timeout.timeout + currentTime;
		timeoutActions.Add (timeout);
	}
}

public class TickAction
{
	public Action behavior;
	public float tickTime;
	public float nextCallTime = 0;

	public TickAction(float tickTime, Action behavior)
	{
		this.tickTime = tickTime;
		this.behavior = behavior;
		nextCallTime += tickTime;
	}
}

public class TimeoutAction
{
	public Action behavior;
	public float timeout;
	public float nextCallTime = 0f;
	public bool isCalled = false;

	public TimeoutAction(float timeout, Action behavior)
	{
		this.timeout = timeout;
		this.behavior = behavior;
	}
}