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
        else
            Debug.LogWarning("Something which is not lemming triggers TouchTrigger. " + collider.gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var lemming = collider.gameObject.GetComponent<Lemming>();

        if (lemming != null)
            lemmings.Add(lemming);
        else
            Debug.LogWarning("Something which is not lemming exits from TouchTrigger. " + collider.gameObject.name);
    }

    public List<Lemming> Lemmings
    {
        get
        {
            return lemmings;
        }
    }
}
