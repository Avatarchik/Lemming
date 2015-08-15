using UnityEngine;
using UnityEngine.UI;
using System;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    private Text timeText;
    private float currentTime = 0;
    private bool timerStarted = false;
    
    void FixedUpdate() {
        if (timerStarted) {
            currentTime += Time.deltaTime;
            SetTimeText(currentTime);
        }
    }

    public void StartTimer() {
        timerStarted = true;
    }
    
    public void StopTimer() {
        timerStarted = false;
    }
    
    public void ResetTime() {
        currentTime = 0;
        SetTimeText(currentTime);
    }

    private void SetTimeText(float sec) {
		TimeSpan time = TimeSpan.FromSeconds(sec);
		timeText.text = string.Format("{0:D2}:{1:D2}:{2:D3}", time.Minutes, time.Seconds, time.Milliseconds);
    }
}