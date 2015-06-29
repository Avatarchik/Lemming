using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lemming : MonoBehaviour
{
    public float speed;
    public float defaultSpeed;
    private IState currentState;
    private Vector2? currentTargetPosition;
    private Queue<Vector2> targetPositionQueue = new Queue<Vector2>();

    public enum Action
    {
        None,
        Idle,
        MoveToCliff,
        BackToCenter,
        JumpToCliff,
        GameOver
    }

    public enum State
    {
        None,
        Idle,
        MoveToCliff,
        FindAvailableCliff,
        BackToCenter,
        JumpToCliff,
        Die
    }

    void Awake()
    {
        speed = 3f;
        defaultSpeed = 3f;
    }

    void Update()
    {
        if (currentState != null)
            currentState.Update();
    }

    private bool IsPossibleAction(Action action)
    {
        // FIXME: It'll be implemented.
        return true;
    }

    public void ResetSpeed()
    {
        defaultSpeed = 3f;
    }

    public void ChangeAction(Action action)
    {
        if (!IsPossibleAction(action))
        {
            Debug.Log("Invalid state");
            return;
        }

        switch (action)
        {
            case Action.Idle:
                currentState = new IdleState(this);
                break;
            case Action.MoveToCliff:
                if (targetPositionQueue.Count == 0)
                {
                    currentTargetPosition = null;
                    currentState = new FindAvailableCliffState(this);
                }
                else
                {
                    currentTargetPosition = targetPositionQueue.Dequeue();
                    currentState = new MoveToCliffState(this, currentTargetPosition.Value);
                }
                break;
            case Action.BackToCenter:
                currentState = new BackToCenterState(this);
                break;
            case Action.JumpToCliff:
                currentState = new JumpToCliffState(this);
                break;
            case Action.GameOver:
                currentState = new GameOverState(this);
                break;
            default:
                break;
        }
    }

    public State GetCurrentState()
    {
        if (currentState is IdleState)
            return State.Idle;
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
        else
            return State.None;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("CliffTrigger"))
            ChangeAction(Action.JumpToCliff);
    }

    public void EnqueueTargetPosition(Vector2 targetPosition)
    {
        targetPositionQueue.Enqueue(targetPosition);
    }

    public void ResetTargetPositionQueue()
    {
        targetPositionQueue.Clear();
    }

    public void IncreaseSpeed(float deltaSpeed)
    {
        defaultSpeed += deltaSpeed;
    }
}
