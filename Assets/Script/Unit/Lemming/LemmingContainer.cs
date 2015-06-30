using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LemmingContainer
{
    private GameObject[] lemmingObjects;
    private Vector2[] spawnPosition;
    private readonly string LemmingResourcePath = "Unit/Lemming/Lemming";

    public LemmingContainer()
    {
        lemmingObjects = new GameObject[LemmingConfig.CountOfLemmings];
        spawnPosition = new[] {
            new Vector2 (0f, 0.8f),
            new Vector2 (-0.4f, 0.4f),
            new Vector2 (0.4f, 0.4f),
            new Vector2 (-0.8f, 0f),
            new Vector2 (0f, 0f),
            new Vector2 (0.8f, 0f),
            new Vector2 (-0.4f, -0.4f),
            new Vector2 (0.4f, -0.4f),
            new Vector2 (0f, -0.8f)
        };
    }

    public void SpawnLemmings()
    {
        for (var i = 0; i < lemmingObjects.Length; i++)
        {
            // FIXME : Each lemmings will have different resource.
            lemmingObjects[i] = GameObject.Instantiate(Resources.Load(LemmingResourcePath) as GameObject);
            lemmingObjects[i].name = "Lemming" + i;
            lemmingObjects[i].transform.position = spawnPosition[i];
            lemmingObjects[i].transform.parent = Camera.main.transform;
            lemmingObjects[i].GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }

    public bool IsAnyLemmingFindingCliff()
    {
        return lemmingObjects.Any(lemmingObject => lemmingObject.GetComponent<Lemming>().GetCurrentState() == Lemming.State.FindAvailableCliff);
    }

    public GameObject[] LemmingObjects
    {
        get
        {
            return lemmingObjects;
        }
    }

    public void ResetLemmingPosition()
    {
        for (var i = 0; i < lemmingObjects.Length; i++)
            lemmingObjects[i].transform.position = spawnPosition[i];
    }

    public void ResetLemmingState()
    {
        lemmingObjects.ToList().ForEach(lemmingObject => lemmingObject.GetComponent<Lemming>().ChangeAction(Lemming.Action.Idle));
    }
    
    public void ChangeToGameOverState()
    {
        lemmingObjects.ToList().ForEach(lemmging => lemmging.GetComponent<Lemming>().ChangeAction(Lemming.Action.GameOver));
    }

    public void BroadcastToFindNewTargetToAllLemmings(HexagonMap.MapPosition targetPositionIndex)
    {
        lemmingObjects.ToList().ForEach(lemmingObject => {
            var lemming = lemmingObject.GetComponent<Lemming>();
            lemming.EnqueueTargetPositionIndex(targetPositionIndex);
            if (lemming.GetCurrentState() == Lemming.State.WaitForFindingCliff)
                lemming.ChangeAction(Lemming.Action.MoveToCliff);
        });
    }

    public void ResetTargetPositionIndexQueue()
    {
        lemmingObjects.ToList().ForEach(lemmingObject => lemmingObject.GetComponent<Lemming>().ResetTargetPositionIndexQueue());
    }

    public void IncreaseLemmingSpeed(float deltaSpeed)
    {
        lemmingObjects.ToList().ForEach(lemmingObject => lemmingObject.GetComponent<Lemming>().IncreaseSpeed(deltaSpeed));
    }
    
    public void ResetLemmingSpeed()
    {
        lemmingObjects.ToList().ForEach(lemmingObject => lemmingObject.GetComponent<Lemming>().ResetSpeed());
    }
}
