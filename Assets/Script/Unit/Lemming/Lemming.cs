using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lemming : MonoBehaviour
{
	public float defaultSpeed;
	public float speed;
	public float power;
	private List<Vector2> availableCliffPositionList = new List<Vector2> ();
	private Vector2 centerPosition;
	private IState currentState;

	public enum Action
	{
		None,
		Idle,
		MoveToCliff,
		BackToCenter,
		JumpToCliff,
		Die
	}

	public enum State
	{
		None,
		Idle,
		MoveToCliff,
		BackToCenter,
		JumpToCliff,
		Die
	}

	void Awake ()
	{
		// FIXME: read speed and power information from table.
		defaultSpeed = 2.5f;
		speed = 3f;
		power = 1.5f;
	}

	void Update ()
	{
		if (currentState != null)
			currentState.Update ();
	}

	private bool IsPossibleAction (Action action)
	{
		// FIXME: It'll be implemented.
		return true;
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
		case Action.MoveToCliff:
			currentState = new MoveToCliffState (this);
			break;
		case Action.BackToCenter:
			currentState = new BackToCenterState (this);
			break;
		case Action.JumpToCliff:
			currentState = new JumpToCliffState (this);
			break;
		default:
			break;
		}
	}

	public State GetCurrentState ()
	{
		if (currentState is IdleState)
			return State.Idle;
		else if (currentState is MoveToCliffState)
			return State.MoveToCliff;
		else if (currentState is BackToCenterState)
			return State.BackToCenter;
		else if (currentState is JumpToCliffState)
			return State.JumpToCliff;
		else if (currentState is DieState)
			return State.Die;
		else
			return State.None;
	}

	public List<Vector2> AvailableCliffPositionList {
		get {
			return availableCliffPositionList;
		}
		set {
			availableCliffPositionList = new List<Vector2> (value);
		}
	}

	public Vector2 CenterPosition {
		get {
			return centerPosition;
		}
		set {
			centerPosition = value;
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		ChangeAction (Action.JumpToCliff);
	}
}
