using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnButton : MonoBehaviour
{
    public float turnDir;
    private bool heldDown;

    void Update ()
    {
        if(heldDown)
            Car.instance.Turn(turnDir);
    }

    public void OnDown ()
    {
        heldDown = true;
    }

    public void OnUp ()
    {
        heldDown = false;
    }
}