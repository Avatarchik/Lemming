using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainHud;
    [SerializeField]
    private GameObject restartButton;
    [SerializeField]
    private GameObject readyCounter;
    [SerializeField]
    private GameObject optionPanel;
    [SerializeField]
    private GameTimer timer;
    [SerializeField]
    private GameObject mapObject;
    private Map map;
    private GameState currentGameState;
    private LemmingContainer lemmingContainer;

    public enum GameState
    {
        Ready,
        Start,
        GameOver,
        Pause
    }

    public static GameController Instance
    {
        get;
        private set;
    }

    public GameState CurrentGameState
    {
        get
        {
            return currentGameState;
        }
    }

    void Awake()
    {
        Instance = this;
        currentGameState = GameState.Ready;
        Initialize();
    }

    void Start()
    {

    }

    [UnityEventListener]
    private void StartGame()
    {
        ShowMainHud();
        timer.ResetTime();
        StartCoroutine(ShowReadyCounter(StartCounterCallback));
    }

    [UnityEventListener]
    private void OpenOptionPanel()
    {
        PauseGame();
        ShowOptionPanel();
    }

    [UnityEventListener]
    private void CloseOptionMenu()
    {
        ContinueGame();
        HideOptionPanel();
    }

    private void PauseGame()
    {
        currentGameState = GameState.Pause;
        Time.timeScale = 0;
    }

    private void ContinueGame()
    {
        currentGameState = GameState.Start;
        Time.timeScale = 1;
    }

    private void ShowOptionPanel()
    {
        optionPanel.SetActive(true);
    }

    private void HideOptionPanel()
    {
        optionPanel.SetActive(false);
    }

    private IEnumerator ShowReadyCounter(Action counterCallback = null)
    {
        readyCounter.SetActive(true);
        readyCounter.GetComponentInChildren<Text>().text = "3";
        yield return new WaitForSeconds(1f);
        readyCounter.GetComponentInChildren<Text>().text = "2";
        yield return new WaitForSeconds(1f);
        readyCounter.GetComponentInChildren<Text>().text = "1";
        yield return new WaitForSeconds(1f);
        readyCounter.SetActive(false);

        if (counterCallback != null)
            counterCallback();
    }

    private void StartCounterCallback()
    {
        currentGameState = GameState.Start;
        timer.StartTimer();
        StartCoroutine("CheckingLemmingState");
        StartCoroutine("IncreaseGameLevel");
    }

    private void ShowMainHud()
    {
        mainHud.SetActive(true);
    }

    private IEnumerator CheckingLemmingState()
    {
        var lemmings = lemmingContainer.LemmingObjects;
        while (currentGameState == GameState.Start)
        {
            for (var i = 0; i < lemmings.Length; i++)
            {
                var lemming = lemmings[i].GetComponent<Lemming>();
                switch (lemming.GetCurrentState())
                {
                    case Lemming.State.Idle:
                        lemming.ChangeAction(Lemming.Action.MoveToCliff);
                        break;
                    case Lemming.State.MoveToCliff:
                        break;
                    case Lemming.State.FindAvailableCliff:
                        break;
                    case Lemming.State.BackToCenter:
                        break;
                    case Lemming.State.JumpToCliff:
                        GameOver();
                        break;
                    case Lemming.State.Die:
                        break;
                    default:
                        break;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    [UnityEventListener]
    private void RestartGame()
    {
        lemmingContainer.ResetLemmingPosition();
        lemmingContainer.ResetLemmingState();
        lemmingContainer.ResetTargetPositionQueue();
        lemmingContainer.ResetLemmingSpeed();
        StartGame();
    }

    private void GameOver()
    {
        lemmingContainer.ChangeToGameOverState();
        currentGameState = GameState.GameOver;
        StopCoroutine("IncreaseGameLevel");
        StopCoroutine("CheckingLemmingState");
        restartButton.SetActive(true);
        timer.StopTimer();
    }

    void Update()
    {
    }

    private IEnumerator IncreaseGameLevel()
    {
        const float fixedTickTime = 10f;
        const float increasingSpeedValue = 0.5f;

        while(true)
        {
            yield return new WaitForSeconds(fixedTickTime);
            lemmingContainer.IncreaseLemmingSpeed(increasingSpeedValue);
        }
    }

    private void Initialize()
    {
        InitializeMap();
        InitializeLemmings();
    }

    private void InitializeMap()
    {
        map = mapObject.GetComponent<Map>();
    }

    private void InitializeLemmings()
    {
        lemmingContainer = new LemmingContainer();
        lemmingContainer.SpawnLemmings();
        lemmingContainer.ResetLemmingState();
    }

    public List<Vector2> GetAvailableCliffPosition()
    {
        // FIXME: It'll be implemented.
        return map.GetCliffPosition();
    }

    public List<Vector2> GetCliffPosition()
    {
        return map.GetCliffPosition();
    }

    public void BroadcastToFindNewTargetToAllLemmings(Vector2 targetPosition)
    {
        lemmingContainer.BroadcastToFindNewTargetToAllLemmings(targetPosition);
    }

    public Vector2 GetCenterPosition()
    {
        return map.GetCenterPosition();
    }

    public void TouchInputTrigger(GameObject trigger)
    {
        map.TouchInputTrigger(trigger);
    }
}
