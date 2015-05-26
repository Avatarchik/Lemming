using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchTriggerAction : MonoBehaviour
{
    private List<Lemming> lemmings = new List<Lemming>();

    void OnTriggerExit2D(Collider2D collider)
    {
        var lemming = collider.gameObject.GetComponent<Lemming>();

        if (lemming != null)
            lemmings.Remove(lemming);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var lemming = collider.gameObject.GetComponent<Lemming>();

        if (lemming != null)
            lemmings.Add(lemming);
    }

    public List<Lemming> Lemmings
    {
        get
        {
            return lemmings;
        }
    }
}
