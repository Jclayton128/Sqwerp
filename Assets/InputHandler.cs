using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //param
    float aimNullZone = 45;
    
    //state
    public float CommandedForwardSignal { get; protected set; }
    public float CommandedStrafeSignal { get; protected set; }
    public float CommandedPitchSignal { get; protected set; }
    public float CommandedYawSignal { get; protected set; }
    public Vector3 aimPos { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    
}
