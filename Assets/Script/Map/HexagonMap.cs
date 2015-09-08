using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CliffPosition
{
   public GameObject[] cliffPositions;
}

public class HexagonMap : MonoBehaviour
{
    public enum MapPosition {
        UpLeft = 0,
        Up,
        UpRight,
        DownRight,
        Down,
        DownLeft,
    }

	[SerializeField] public Transform pelicanSpawnPoint;
	[SerializeField] public Transform pelicanTargetPoint;
	[SerializeField] public Transform orcaSpawnPoint;
	[SerializeField] public Transform thunderSpawnPoint;

    [SerializeField] private GameObject [] touchTriggers;
    [SerializeField]
    private CliffPosition[] cliffPositions;
    private Vector2 centerPosition;
    public HexagonMap()
    {
        centerPosition = new Vector2(0, 0);
    }

    public Vector2 GetCenterPosition()
    {
        return centerPosition;
    }

	public Vector2 GetRandomPositionInCliffPosition(MapPosition position)
    {
        var cliff = cliffPositions[(int)position];
        return cliff.cliffPositions[Random.Range(0, cliff.cliffPositions.Length)].transform.position;
    }
    
    public MapPosition GetRandomMapPosition()
    {
        return (MapPosition)Random.Range (0, cliffPositions.Length);
    }

    public void TouchInputTrigger(GameObject trigger)
    {
        var lemmings = trigger.GetComponent<TouchTriggerAction>().Lemmings;
        lemmings.ForEach(lemming => lemming.ChangeAction(Lemming.Action.BackToCenter));
    }
}
