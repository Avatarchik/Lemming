using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class MoveToCliffState : IState
{
    private Vector2 targetPosition;
    private Lemming lemming;

    public MoveToCliffState(Lemming lemming, Vector2 targetPosition)
    {
        this.lemming = lemming;
        this.targetPosition = targetPosition;
        SetRandomSpeed(ref lemming);
    }

    private void SetRandomSpeed(ref Lemming lemming)
    {
        const float RANDOM_PERCENTAGE_LIMIT = 20;
        var maximumSpeed = lemming.defaultSpeed * (1 + RANDOM_PERCENTAGE_LIMIT / 100);
        var minimumSpeed = lemming.defaultSpeed * (1 - RANDOM_PERCENTAGE_LIMIT / 100);
        lemming.speed = Random.Range(minimumSpeed, maximumSpeed);
    }

    public void Update()
    {
        if (IsValidTargetPosition())
            MoveToTargetCliff();
        else
            MoveToCenter();
    }

    private void MoveToCenter()
    {
        lemming.ChangeAction(Lemming.Action.BackToCenter);
    }

    private bool IsValidTargetPosition()
    {
        return GameController.Instance.GetAvailableCliffPosition().Contains(targetPosition);
    }

    private void MoveToTargetCliff()
    {
        float step = lemming.speed * Time.deltaTime;
        lemming.transform.position = Vector3.MoveTowards(lemming.transform.position, targetPosition, step);
    }
}
