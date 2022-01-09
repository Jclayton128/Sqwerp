using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (SpeedHandler), typeof (InputHandler), typeof (Rigidbody))]
[RequireComponent (typeof (Collider))]
public class MovementHandler : MonoBehaviour
{
    SpeedHandler sh;
    InputHandler ih;
    Rigidbody rb;
    Collider coll;

    //param
    float hoverDistance_y = 1.5f;

    //state
    float distFromSurface;

    // Start is called before the first frame update
    void Start()
    {
        sh = GetComponent<SpeedHandler>();
        ih = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    private void Update()
    {
        CalculateHoverDistance();
    }

    private void CalculateHoverDistance()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -1 * transform.up, out hit);
        distFromSurface = hit.distance;
    }

    private void FixedUpdate()
    {
        HandleForwardMovement();
        HandleStrafeMovement();

        //if (Mathf.Abs(ih.CommandedStrafeSignal * ih.CommandedForwardSignal) <= Mathf.Epsilon)
        //{
        //    rb.velocity = Vector3.zero;
        //}

        
        HoverOffSurface();

    }

    private void HoverOffSurface()
    {
        if (distFromSurface < hoverDistance_y)
        {
            Vector3 hoverPos = new Vector3(0, hoverDistance_y - distFromSurface, 0);
            transform.position += hoverPos;
        }

    }

    private void HandleStrafeMovement()
    {
        if (Mathf.Abs(ih.CommandedStrafeSignal) > 0)
        {
            rb.AddForce(ih.CommandedStrafeSignal * transform.right * sh.GetStrafeVelocity());
        }


    }

    private void HandleForwardMovement()
    {
        if (ih.CommandedForwardSignal > 0)
        {
            rb.AddForce(transform.forward * sh.GetForwardVelocity());
        }
        if (ih.CommandedForwardSignal < 0)
        {
            rb.AddForce(-1 * transform.forward * sh.GetReverseVelocity());
        }
    }
}
