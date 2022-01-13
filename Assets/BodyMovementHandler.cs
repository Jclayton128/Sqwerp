using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (SpeedHandler), typeof (InputHandler), typeof (Rigidbody))]
public class BodyMovementHandler : MonoBehaviour
{
    SpeedHandler sh;
    InputHandler ih;
    Rigidbody rb;
    BoxCollider coll;
    HeadMovementHandler hmh;

    //param
    float maxAdhesionDist = 2f;
    float adhesionForce = 20f;
    float adjustToNewSurfaceNormalRate = 180f;
    Vector3 surfaceRaycastDirection_moving = new Vector3(0, -1, 1);
    Vector3 surfaceRaycastDirection_still = new Vector3(0, -1, 0);

    //state
    float requestedYaw;
    public float distFromSurface;
    Vector3 surfaceNormal;
    public bool isOnSurface;

    // Start is called before the first frame update
    void Start()
    {
        sh = GetComponent<SpeedHandler>();
        ih = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
        hmh = GetComponentInChildren<HeadMovementHandler>();
    }

    private void Update()
    {
        CalculateHoverDistance();
    }

    private void CalculateHoverDistance()
    {
        RaycastHit hit = new RaycastHit();
        Vector3 raydir = surfaceRaycastDirection_moving;

        //if (ih.CommandedForwardSignal <= 0)
        //{
        //    raydir = surfaceRaycastDirection_still;
        //}
        //if (ih.CommandedForwardSignal > 0)
        //{
        //    raydir = surfaceRaycastDirection_moving;

        //}


        Physics.Linecast(transform.position, transform.position + (raydir * maxAdhesionDist), out hit) ;
        Debug.DrawLine(transform.position, transform.position + (raydir * maxAdhesionDist), Color.yellow, 0.1f);
        if (hit.collider)
        {
            //Debug.DrawLine(transform.position, hit.point, Color.yellow, 0.1f);
            isOnSurface = true;
            distFromSurface = hit.distance;
            surfaceNormal = hit.normal;
        }
        else
        {
            //Debug.DrawRay(transform.position, raydir, Color.yellow, 0.1f);
            isOnSurface = false;
            distFromSurface = 99f;
            surfaceNormal = Vector3.up;
        }

        //Debug.DrawLine(transform.position, transform.position + surfaceNormal, Color.green);
    }

    private void FixedUpdate()
    {
        HandleJump();
        HandleForwardMovement();
        //HandleStrafeMovement();

        HandleTurn();
        //HandleAim();
        HoverOffSurfaceNormal();


    }

    private void HandleTurn()
    {
        if (!isOnSurface) { return; }
        float signal = ih.CommandedYawSignal;
        if (Mathf.Abs(signal) > 0)
        {
            transform.Rotate(transform.up, signal * sh.GetRotationRate() * Time.deltaTime, Space.Self);
        }

    }

    private void HandleJump()
    {
        if (ih.JumpRequested && isOnSurface)
        {
            rb.AddRelativeForce(sh.GetJumpVector(), ForceMode.Impulse);
        }
    }

    public void SetRequestedYaw(float requestedYaw)
    {
        this.requestedYaw = requestedYaw;
    }
    private void HandleAim()
    {
        if (Mathf.Abs(requestedYaw) > 0)
        {
            transform.Rotate(transform.up, requestedYaw * sh.GetRotationRate() * Time.deltaTime, Space.Self);
        }
    }

    private void HoverOffSurfaceNormal()
    {
        if (isOnSurface)
        {
            rb.useGravity = false;
            rb.AddForce(-1 * surfaceNormal * adhesionForce); //* (distFromSurface - maxAdhesionDist));
            Debug.DrawLine(transform.position,
                transform.position + (-1 * surfaceNormal * adhesionForce * (distFromSurface - maxAdhesionDist)),
                Color.cyan) ;
            //transform.up = Vector3.Lerp(transform.up, surfaceNormal, Time.deltaTime);

            Quaternion deltaRot = Quaternion.FromToRotation(this.transform.up, surfaceNormal);
            Quaternion targRot = deltaRot * this.transform.rotation;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targRot, Time.deltaTime * adjustToNewSurfaceNormalRate);

        }
        else
        {
            rb.useGravity = true;
        }

    }

    private void HandleStrafeMovement()
    {
        if (Mathf.Abs(ih.CommandedYawSignal) > 0)
        {
            rb.AddForce(ih.CommandedYawSignal * transform.right * sh.GetStrafeVelocity(), ForceMode.Impulse);

        }


    }

    private void HandleForwardMovement()
    {
        if (!isOnSurface) { return; }
        if (ih.CommandedForwardSignal > 0)
        {
            rb.AddForce(transform.forward * sh.GetForwardVelocity(), ForceMode.Impulse);
        }
        if (ih.CommandedForwardSignal < 0)
        {
            rb.AddForce(-1 * transform.forward * sh.GetReverseVelocity(), ForceMode.Impulse);
        }
    }
}
