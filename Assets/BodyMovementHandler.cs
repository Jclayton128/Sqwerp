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
    float hoverDistance_y = 1.5f;
    float hoverForce = 20f;
    float adjustToNewSurfaceNormalRate = 180f;

    //state
    public float deltaFromSurface_pitch;
    public float deltaFromSurface_roll;
    float requestedYaw;
    float distFromSurface;
    Vector3 surfaceNormal;

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
        RaycastHit hit;
        Physics.Raycast(transform.position, -1 * transform.up, out hit);
        distFromSurface = hit.distance;
        surfaceNormal = hit.normal;
        Debug.DrawLine(transform.position, transform.position + surfaceNormal, Color.green);
    }

    private void FixedUpdate()
    {
        HandleForwardMovement();
        HandleStrafeMovement();
        HandleAim();
        HoverOffSurfaceNormal();


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
        rb.AddRelativeForce(transform.up * (hoverDistance_y - distFromSurface) * hoverForce);
        //transform.up = Vector3.Lerp(transform.up, surfaceNormal, Time.deltaTime);

        Quaternion deltaRot = Quaternion.FromToRotation(this.transform.up, surfaceNormal);
        Quaternion targRot = deltaRot * this.transform.rotation;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targRot, Time.deltaTime * adjustToNewSurfaceNormalRate);


    }

    private void HandleStrafeMovement()
    {
        if (Mathf.Abs(ih.CommandedStrafeSignal) > 0)
        {
            rb.AddForce(ih.CommandedStrafeSignal * transform.right * sh.GetStrafeVelocity(), ForceMode.Impulse);

        }


    }

    private void HandleForwardMovement()
    {
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
