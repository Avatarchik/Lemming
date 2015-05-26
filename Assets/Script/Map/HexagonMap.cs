using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexagonMap : MonoBehaviour, Map
{
    [SerializeField] GameObject [] touchTriggers;
    private List<Vector2> cliffPosition;
    private Vector2 centerPosition;
    public HexagonMap()
    {
        cliffPosition = new List<Vector2> {
            new Vector2 (0, 4.3f),
            new Vector2 (3.6f, 2.4f),
            new Vector2 (3.6f, -2.4f),
            new Vector2 (0, -4.3f),
            new Vector2 (-3.6f, -2.4f),
            new Vector2 (-3.6f, 2.4f)
        };
        centerPosition = new Vector2(0, 0);
    }

    Vector2 Map.GetCenterPosition()
    {
        return centerPosition;
    }

    List<Vector2> Map.GetCliffPosition()
    {
        return cliffPosition;
    }
    
    void Map.TouchInputTrigger(GameObject trigger)
    {
        var lemmings = trigger.GetComponent<TouchTriggerAction>().Lemmings;
        foreach (var lemming in lemmings)
            lemming.ChangeAction(Lemming.Action.BackToCenter);
    }
}
