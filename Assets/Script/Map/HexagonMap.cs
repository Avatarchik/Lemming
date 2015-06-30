using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexagonMap : MonoBehaviour
{
    public enum MapPosition {
        UpLeft = 0,
        Up,
        UpRight,
        DownLeft,
        Down,
        DownRight
    }
    
    [SerializeField] GameObject [] touchTriggers;
    private Vector2 [] cliffPosition;
    private Vector2 centerPosition;
    public HexagonMap()
    {
        cliffPosition = new Vector2 [] {
            new Vector2 (-3.6f, 2.4f),
            new Vector2 (0, 4.3f),
            new Vector2 (3.6f, 2.4f),
            new Vector2 (-3.6f, -2.4f),
            new Vector2 (0, -4.3f),
            new Vector2 (3.6f, -2.4f),
        };
        centerPosition = new Vector2(0, 0);
    }

    public Vector2 GetCenterPosition()
    {
        return centerPosition;
    }

    public Vector2[] GetCliffPosition()
    {
        return cliffPosition;
    }
    
    public void TouchInputTrigger(GameObject trigger)
    {
        var lemmings = trigger.GetComponent<TouchTriggerAction>().Lemmings;
        foreach (var lemming in lemmings)
            lemming.ChangeAction(Lemming.Action.BackToCenter);
    }
}
