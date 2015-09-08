using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Lemming : MonoBehaviour
{
	public float speed;
	public float defaultSpeed;
	private IState currentState;
	private HexagonMap.MapPosition? currentTargetPositionIndex;
	public Vector2? randomPositionOfCurrentTargetPosition;
	private bool isStunState = false;
	private Queue<HexagonMap.MapPosition> targetPositionQueue = new Queue<HexagonMap.MapPosition> ();

	public void SetStunState()
	{
		isStunState = true;
	}

	public void ResetStunState()
	{
		isStunState = false;
	}

	public enum Action
	{
		None,
		Idle,
		ReadyToRunInCenter,
		MoveToCliff,
		BackToCenter,
		JumpToCliff,
		GameOver,
		WaitForFindingCliff
	}

	public enum State
	{
		None,
		Idle,
		ReadyToRunInCenter,
		MoveToCliff,
		FindAvailableCliff,
		BackToCenter,
		JumpToCliff,
		WaitForFindingCliff,
		Die
	}

	void Awake ()
	{
		speed = 3f;
		defaultSpeed = 3f;
	}

	void Update ()
	{
		if (currentState != null && GameController.Instance.CurrentGameState != GameController.GameState.Pause && !isStunState)
			currentState.Update ();
	}

	private bool IsPossibleAction (Action action)
	{
		// FIXME: It'll be implemented.
		return true;
	}

	public void ResetSpeed ()
	{
		defaultSpeed = 3f;
	}

	public void ChangeAction (Action action)
	{
		if (!IsPossibleAction (action)) {
			Debug.Log ("Invalid state");
			return;
		}

		switch (action) {
		case Action.Idle:
			currentState = new IdleState (this);
			break;
		case Action.ReadyToRunInCenter:
			currentState = new ReadyToRunInCenterState (this);
			break;
		case Action.MoveToCliff:
			if (targetPositionQueue.Count == 0) {
				currentTargetPositionIndex = null;
				currentState = new FindAvailableCliffState (this);
			} else {
				currentTargetPositionIndex = targetPositionQueue.Dequeue ();
				currentState = new MoveToCliffState (this, currentTargetPositionIndex.Value);
			}
			break;
		case Action.BackToCenter:
			currentState = new BackToCenterState (this);
			break;
		case Action.JumpToCliff:
			currentState = new JumpToCliffState (this);
			break;
		case Action.GameOver:
			currentState = new GameOverState (this);
			break;
		case Action.WaitForFindingCliff:
			currentState = new WaitForFindingCliffState (this);
			break;
		default:
			break;
		}

		SwitchLemmingAnimation (action);
	}

	private void InitializeAnimationParam ()
	{
		var animator = GetComponent<Animator> ();
		animator.SetBool ("readyToRunInCenter", false);
		animator.SetBool ("back", false);
		animator.SetBool ("run_up_left", false);
		animator.SetBool ("run_up", false);
		animator.SetBool ("run_up_right", false);
		animator.SetBool ("run_down_left", false);
		animator.SetBool ("run_down", false);
		animator.SetBool ("run_down_right", false);
		animator.SetBool ("toIdle", false);
		animator.SetBool ("gameOver", false);
	}

	private void SwitchLemmingAnimation (Action action)
	{
		InitializeAnimationParam ();
		var animator = GetComponent<Animator> ();
		switch (action) {
		case Action.Idle:
			animator.SetBool ("toIdle", true);
			break;
		case Action.ReadyToRunInCenter:
			animator.SetBool ("readyToRunInCenter", true);
			break;
		case Action.MoveToCliff:
			animator.SetBool ("run_up_left", currentTargetPositionIndex.HasValue && currentTargetPositionIndex.Value == HexagonMap.MapPosition.UpLeft);
			animator.SetBool ("run_up", currentTargetPositionIndex.HasValue && currentTargetPositionIndex.Value == HexagonMap.MapPosition.Up);
			animator.SetBool ("run_up_right", currentTargetPositionIndex.HasValue && currentTargetPositionIndex.Value == HexagonMap.MapPosition.UpRight);
			animator.SetBool ("run_down_left", currentTargetPositionIndex.HasValue && currentTargetPositionIndex.Value == HexagonMap.MapPosition.DownLeft);
			animator.SetBool ("run_down", currentTargetPositionIndex.HasValue && currentTargetPositionIndex.Value == HexagonMap.MapPosition.Down);
			animator.SetBool ("run_down_right", currentTargetPositionIndex.HasValue && currentTargetPositionIndex.Value == HexagonMap.MapPosition.DownRight);
			break;
		case Action.BackToCenter:
			animator.SetBool ("back", true);
			break;
		case Action.JumpToCliff:
			animator.SetBool ("gameOver", true);
			break;
		case Action.GameOver:
			break;
		case Action.WaitForFindingCliff:
			break;
		default:
			break;
		}
	}

	public State GetCurrentState ()
	{
		if (currentState is IdleState)
			return State.Idle;
		else if (currentState is ReadyToRunInCenterState)
			return State.ReadyToRunInCenter;
		else if (currentState is MoveToCliffState)
			return State.MoveToCliff;
		else if (currentState is BackToCenterState)
			return State.BackToCenter;
		else if (currentState is FindAvailableCliffState)
			return State.FindAvailableCliff;
		else if (currentState is JumpToCliffState)
			return State.JumpToCliff;
		else if (currentState is DieState)
			return State.Die;
		else if (currentState is GameOverState)
			return State.Die;
		else if (currentState is WaitForFindingCliffState)
			return State.WaitForFindingCliff;
		else {
			Debug.LogWarning ("Not Reach here " + currentState.GetType ().FullName);
			return State.None;
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.name.Contains ("CliffTrigger"))
			ChangeAction (Action.JumpToCliff);
	}

	public void EnqueueTargetPositionIndex (HexagonMap.MapPosition targetPosition)
	{
		targetPositionQueue.Enqueue (targetPosition);
	}

	public void ResetTargetPositionIndexQueue ()
	{
		targetPositionQueue.Clear ();
	}

	public void IncreaseSpeed (float deltaSpeed)
	{
		defaultSpeed += deltaSpeed;
	}
}
