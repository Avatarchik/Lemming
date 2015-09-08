using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using ConditionalAttribute = System.Diagnostics.ConditionalAttribute;

public class GameController : MonoBehaviour
{
	[SerializeField]
	private GameObject
		quitPopup;
	[SerializeField]
	private GameObject
		mainHud;
	[SerializeField]
	private GameObject
		gameOverPanel;
	[SerializeField]
	private GameObject
		readyCounter;
	[SerializeField]
	private GameObject
		optionPanel;
	[SerializeField]
	private GameTimer
		timer;
	[SerializeField]
	private GameObject
		mainMenu;
	public HexagonMap map;
	private GameState currentGameState;
	private LemmingContainer lemmingContainer;

	public GameState CurrentGameState {
		get {
			return currentGameState;
		}
	}

	public enum GameState
	{
		Ready,
		Start,
		GameOver,
		Pause
	}

	public enum AnimalFriendType
	{
		Pelican = 0,
		Orca
	}

	public static GameController Instance {
		get;
		private set;
	}

	void Awake ()
	{
		Instance = this;
		currentGameState = GameState.Ready;
		Initialize ();
	}

	void Start ()
	{

	}

	[UnityEventListener]
	private void StartGame ()
	{
		ShowMainHud ();
		timer.ResetTime ();
		StartCoroutine (ShowReadyCounter (StartCounterCallback));
	}

	private void HideMainHud ()
	{
		mainHud.SetActive (false);
	}

	[UnityEventListener]
	private void OpenOptionPanel ()
	{
		PauseGame ();
		ShowOptionPanel ();
	}

	void OnApplicationPause ()
	{
		if (currentGameState == GameState.Start) {
			OpenOptionPanel();
		}
	}

	[UnityEventListener]
	private void CloseOptionMenu ()
	{
		ContinueGame ();
		HideOptionPanel ();
	}

	private void PauseGame ()
	{
		currentGameState = GameState.Pause;
		timer.StopTimer ();
	}

	private void ContinueGame ()
	{
		StartCoroutine (ShowReadyCounter (() => {
			timer.StartTimer ();
			currentGameState = GameState.Start;
		}));
	}

	private void ShowOptionPanel ()
	{
		optionPanel.SetActive (true);
	}

	private void HideOptionPanel ()
	{
		optionPanel.SetActive (false);
	}

	private IEnumerator ShowReadyCounter (Action counterCallback = null)
	{
		readyCounter.SetActive (true);
		readyCounter.GetComponentInChildren<Text> ().text = "3";
		yield return new WaitForSeconds (1f);
		readyCounter.GetComponentInChildren<Text> ().text = "2";
		yield return new WaitForSeconds (1f);
		readyCounter.GetComponentInChildren<Text> ().text = "1";
		yield return new WaitForSeconds (1f);
		readyCounter.SetActive (false);

		if (counterCallback != null)
			counterCallback ();
	}

	private void StartCounterCallback ()
	{
		currentGameState = GameState.Start;
		timer.StartTimer ();

		const float increaseGameLevelTickTime = 10f;
		const float spawnAnimalFriendTickTime = 5f;
		timer.RegisterTicker (new TickAction(increaseGameLevelTickTime, IncreaseGameLevel));
		timer.RegisterTicker (new TickAction (spawnAnimalFriendTickTime, SpawnRandomAnimalFriend));
		lemmingContainer.ChangeLemmingStateOfAll (Lemming.Action.ReadyToRunInCenter);
	}

	private void ShowMainHud ()
	{
		mainHud.SetActive (true);
	}

	private void CheckingLemmingState ()
	{
		var lemmings = lemmingContainer.LemmingObjects.Select (go => go.GetComponent<Lemming> ());
		foreach (var lemming in lemmings) {
			switch (lemming.GetCurrentState ()) {
			case Lemming.State.Idle:
				break;
			case Lemming.State.ReadyToRunInCenter:
				if (!lemmingContainer.IsAnyLemmingFindingCliff ())
					lemming.ChangeAction (Lemming.Action.MoveToCliff);
				else
					lemming.ChangeAction (Lemming.Action.WaitForFindingCliff);
				break;
			case Lemming.State.MoveToCliff:
				break;
			case Lemming.State.FindAvailableCliff:
				break;
			case Lemming.State.BackToCenter:
				break;
			case Lemming.State.JumpToCliff:
				GameOver ();
				break;
			case Lemming.State.Die:
				break;
			case Lemming.State.WaitForFindingCliff:
				break;
			default:
				Debug.Assert (false, "Not reach here " + lemming.GetCurrentState ().ToString ());
				break;
			}
		}
	}

	[UnityEventListener]
	private void RestartGame ()
	{
		ResetLemmings ();
		ResetItemBoxes ();
		StartGame ();
	}

	private void ResetItemBoxes()
	{
		ItemController.Instance.ResetItemBoxes ();
	}

	private void ResetLemmings()
	{
		lemmingContainer.ResetLemmingPosition ();
		lemmingContainer.ResetLemmingState ();
		lemmingContainer.ResetTargetPositionIndexQueue ();
		lemmingContainer.ResetLemmingSpeed ();
	}

	private void GameOver ()
	{
		lemmingContainer.ChangeToGameOverState ();
		currentGameState = GameState.GameOver;
		timer.StopTimer ();

		gameOverPanel.GetComponent<GameOverPopup> ().Initialize (timer.GetCurrentTime ());
		gameOverPanel.SetActive (true);
		HideMainHud ();
	}

	void FixedUpdate ()
	{
		// back button
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (currentGameState == GameState.Start)
				OpenOptionPanel();
			else
				quitPopup.SetActive(true);
		}

		CheckingLemmingState ();
		CheckTouchEvent ();
		CheckMouseClickEvent ();
	}

	private Vector2? firstTouchPosition;
	private float touchsensitive = 30f;
	private void CheckTouchEvent()
	{
		if (Input.touchCount == 1)
		{
			var touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				firstTouchPosition = touch.position;
			}

			if (touch.phase == TouchPhase.Ended)
			{
				if (firstTouchPosition.HasValue && Vector3.Magnitude(firstTouchPosition.Value - touch.position) < touchsensitive)
				{
					OnClick(touch.position);
				}

				firstTouchPosition = null;
			}
		}
	}

	private void OnClick(Vector3 clickPosition)
	{
		CheckAnimalFriend (clickPosition);
	}

	private void CheckAnimalFriend(Vector3 clickPosition)
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(clickPosition);
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
		
		if (hit.collider != null) {
			var animalFriend = hit.collider.gameObject.GetComponent<AnimalFriend>();

			if (animalFriend != null)
				animalFriend.OnClicked();
		}
	}

	private Vector3? firstMousePressedPosition; 
	[Conditional("UNITY_EDITOR")]
	private void CheckMouseClickEvent()
	{
		if (Input.GetMouseButtonDown (0))
		{
			firstMousePressedPosition = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp (0))
		{
			if (firstMousePressedPosition.HasValue && Vector3.Magnitude(firstMousePressedPosition.Value - Input.mousePosition) < touchsensitive)
			{
				OnClick(Input.mousePosition);
			}

			firstMousePressedPosition = null;
		}
	}

	private void IncreaseGameLevel ()
	{
		Debug.Log ("Lemming speed Increased..");
		const float increasingSpeedValue = 0.5f;
		lemmingContainer.IncreaseLemmingSpeed (increasingSpeedValue);
	}

	private void Initialize ()
	{
		InitializeLemmings ();
	}

	private void InitializeLemmings ()
	{
		lemmingContainer = new LemmingContainer ();
		lemmingContainer.SpawnLemmings ();
		lemmingContainer.ResetLemmingState ();
	}

	public void BroadcastToFindNewTargetToAllLemmings (HexagonMap.MapPosition targetPosition)
	{
		lemmingContainer.BroadcastToFindNewTargetToAllLemmings (targetPosition);
	}

	public Vector2 GetCenterPosition ()
	{
		return map.GetCenterPosition ();
	}

	public void TouchInputTrigger (GameObject trigger)
	{
		map.TouchInputTrigger (trigger);
	}
	
	[UnityEventListener]
	private void SetGameOver ()
	{
		GameOver ();
	}

	private void GoMainMenu ()
	{
		ResetLemmings ();
		ItemController.Instance.ResetItemBoxes ();

		currentGameState = GameState.Ready;
		mainMenu.SetActive (true);
		mainHud.SetActive (false);
	}

	private void SpawnRandomAnimalFriend()
	{
		Debug.Log ("Spawn animal friend");
		var countOfAnimalFriendType = Enum.GetNames (typeof(AnimalFriendType)).Length;
		var randomAnimalFriend = (AnimalFriendType)UnityEngine.Random.Range (0, countOfAnimalFriendType);

		switch(randomAnimalFriend)
		{
			case AnimalFriendType.Orca:
				Orca.SpawnAnimal ();
				break;
			case AnimalFriendType.Pelican:
				Pelican.SpawnAnimal();
				break;
		}
	}

	public void StunAllLemmingForSecond(float second)
	{
		lemmingContainer.StunAllLemming ();
		timer.RegisterTimeout(new TimeoutAction(second, () => {
			lemmingContainer.ResetStunAllLemming();
		}));
	}

	[UnityEventListener]
	private void QuitLemming()
	{
		Application.Quit ();
	}
}
