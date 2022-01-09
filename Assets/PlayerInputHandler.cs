using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : InputHandler 
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        ListenForKeyboardInput();
    }

    private void ListenForKeyboardInput()
    {
        CommandedForwardSignal = GetForwardSignal();
        CommandedStrafeSignal = GetStrafeSignal();        
    }

    private float GetStrafeSignal()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            return 1;

        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            return -1;
        }

        return 0;
    }

    private float GetForwardSignal()
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            return 1;

        }

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            return -1;
        }

        return 0;
    }
}
