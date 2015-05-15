using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour
{
	[SerializeField] private GameObject restartButton;
	[SerializeField] private GameObject readyCounter;
	private Map map;
	private GameState currentGameState;
	private LemmingContainer lemmingContainer;

	public enum GameState
	{
		Ready,
		Start,
		GameOver
	}

	public static GameController Instance { 
		get;
		private set;
	}

	public GameState CurrentGameState {
		get {
			return currentGameState;
		}
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
		StartCoroutine("ShowReadyCounter");
	}
	
	private IEnumerator ShowReadyCounter() {
		readyCounter.SetActive(true);
		readyCounter.GetComponentInChildren<Text>().text = "3";
		yield return new WaitForSeconds(1f);
		readyCounter.GetComponentInChildren<Text>().text = "2";
		yield return new WaitForSeconds(1f);
		readyCounter.GetComponentInChildren<Text>().text = "1";
		yield return new WaitForSeconds(1f);
		readyCounter.SetActive(false);
		currentGameState = GameState.Start;
		StartCoroutine ("CheckingLemmingState");
	}

	private IEnumerator CheckingLemmingState ()
	{
		var lemmings = lemmingContainer.LemmingObjects;
		while (currentGameState == GameState.Start) {
			for (var i = 0; i < lemmings.Length; i++) {
				var lemming = lemmings [i].GetComponent<Lemming> ();
				switch (lemming.GetCurrentState ()) {
				case Lemming.State.Idle:
					lemming.ChangeAction (Lemming.Action.MoveToCliff);
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
				default:
					break;
				}
			}
			yield return new WaitForFixedUpdate ();
		}
	}

	[UnityEventListener]
	private void RestartGame ()
	{
		lemmingContainer.ResetLemmingPosition ();
		lemmingContainer.ResetLemmingState ();
		lemmingContainer.ResetTargetPositionQueue ();
		StartGame ();
	}

	private void GameOver ()
	{
		currentGameState = GameState.GameOver;
		StopCoroutine ("CheckingLemmingState");
		restartButton.SetActive (true);
	}

	void Update ()
	{

	}

	private void Initialize ()
	{
		InitializeGameData ();
		InitializeLemmings ();
	}

	private void InitializeLemmings ()
	{
		lemmingContainer.SpawnLemmings ();
		lemmingContainer.ResetLemmingState ();
	}

	private void InitializeGameData ()
	{
		// FIXME: Get Map information form global instance.
		map = new HexagonMap ();
		lemmingContainer = new LemmingContainer ();
	}

	public List<Vector2> GetAvailableCliffPosition ()
	{
		// FIXME: It'll be implemented.
		return map.GetCliffPosition ();
	}

	public List<Vector2> GetCliffPosition()
	{
		return map.GetCliffPosition ();
	}

	public void BroadcastToFindNewTargetToAllLemmings (Vector2 targetPosition)
	{
		lemmingContainer.BroadcastToFindNewTargetToAllLemmings (targetPosition);
	}

	public Vector2 GetCenterPosition ()
	{
		return map.GetCenterPosition ();
	}

	public void BackToCenter (Lemming lemming) {
		lemming.ChangeAction (Lemming.Action.BackToCenter);
	}
}
