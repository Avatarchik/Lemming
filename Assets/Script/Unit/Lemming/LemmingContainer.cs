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
        return Lemmings.Any(lemming => lemming.GetCurrentState() == Lemming.State.FindAvailableCliff);
    }

    public GameObject[] LemmingObjects
    {
        get
        {
            return lemmingObjects;
        }
    }

    private List<Lemming> Lemmings
    {
        get
        {
            return lemmingObjects.Select(go => go.GetComponent<Lemming>()).ToList();
        }
    }

    public void ResetLemmingPosition()
    {
        for (var i = 0; i < lemmingObjects.Length; i++)
            lemmingObjects[i].transform.position = spawnPosition[i];
    }

    public void ResetLemmingState()
    {
		Lemmings.ForEach(lemming => {
			lemming.gameObject.SetActive(true);
			lemming.ChangeAction(Lemming.Action.Idle);
			lemming.ResetStunState();
		});
    }
    
    public void ChangeToGameOverState()
    {
        Lemmings.ForEach(lemming=> lemming.ChangeAction(Lemming.Action.GameOver));
    }

	public void ChangeLemmingStateOfAll(Lemming.Action action)
	{
		Lemmings.ForEach (lemming => lemming.ChangeAction (action));
	}

    public void BroadcastToFindNewTargetToAllLemmings(HexagonMap.MapPosition targetPositionIndex)
    {
        Lemmings.ForEach(lemming => {
            lemming.EnqueueTargetPositionIndex(targetPositionIndex);
            if (lemming.GetCurrentState() == Lemming.State.WaitForFindingCliff)
                lemming.ChangeAction(Lemming.Action.MoveToCliff);
        });
    }

    public void ResetTargetPositionIndexQueue()
    {
        Lemmings.ForEach(lemming => lemming.ResetTargetPositionIndexQueue());
    }

    public void IncreaseLemmingSpeed(float deltaSpeed)
    {
        Lemmings.ForEach(lemming => lemming.IncreaseSpeed(deltaSpeed));
    }
    
    public void ResetLemmingSpeed()
    {
        Lemmings.ForEach(lemming => lemming.ResetSpeed());
    }

	public void StunAllLemming()
	{
		Lemmings.ForEach (lemming => lemming.SetStunState ());
	}

	public void ResetStunAllLemming()
	{
		Lemmings.ForEach (lemming => lemming.ResetStunState ());
	}
}
