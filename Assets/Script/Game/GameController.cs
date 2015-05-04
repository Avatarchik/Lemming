using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	[SerializeField] private Lemming lemming;
	[SerializeField] private GameObject restartButton;
	private Map map;
	private GameState currentGameState;

	public enum GameState
	{
		Ready,
		Start,
		GameOver
	}

	void Awake ()
	{
		Initialize ();

		currentGameState = GameState.Ready;
	}

	void Start ()
	{
		
	}

	[UnityEventListener]
	private void StartGame ()
	{
		currentGameState = GameState.Start;
		StartCoroutine ("CheckingLemmingState");
		lemming.ChangeAction (Lemming.Action.MoveToCliff);
	}

	private IEnumerator CheckingLemmingState ()
	{
		while (true) {
			switch (lemming.GetCurrentState ()) {
			case Lemming.State.Idle:
				lemming.ChangeAction (Lemming.Action.MoveToCliff);
				break;
			case Lemming.State.MoveToCliff:

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
			yield return new WaitForFixedUpdate ();
		}
	}

	[UnityEventListener]
	private void RestartGame ()
	{
		ResetLemmingPosition ();
		ResetLemmingState ();
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
		GetMapData ();
		SetCliffAndCenterPositionToLemming ();
		RegisterInputCallback ();
		ResetLemmingPosition ();
		ResetLemmingState ();
	}

	private void GetMapData ()
	{
		// FIXME: Get Map information form global instance.
		map = new HexagonMap ();
	}

	private void ResetLemmingPosition ()
	{
		lemming.transform.position = map.GetCenterPosition ();
	}

	private void ResetLemmingState ()
	{
		lemming.ChangeAction (Lemming.Action.Idle);
	}

	private void SetCliffAndCenterPositionToLemming ()
	{
		lemming.CenterPosition = map.GetCenterPosition ();
		lemming.AvailableCliffPositionList = map.GetCliffPosition ();
	}

	private void InputEnter (Vector2 dir)
	{
		if (currentGameState != GameState.Start)
			return;

		if (lemming.GetCurrentState () == Lemming.State.MoveToCliff)
			lemming.ChangeAction (Lemming.Action.BackToCenter);
	}

	private void RegisterInputCallback ()
	{
		GetComponent<LemmingInputController> ().RegisterInputEventHandler (InputEnter);
	}
}
