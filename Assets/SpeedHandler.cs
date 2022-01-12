using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedHandler : MonoBehaviour
{
    //param
    [SerializeField] float _baseForwardVelocity = 10f;
    [SerializeField] float _baseReverseVelocity = 5f;
    [SerializeField] float _baseStrafeVelocity = 7.5f;
    [SerializeField] float _baseRotationRate = 180f;
    float jumpForce = 10f;
    Vector3 jumpVector = new Vector3(0, 1,0);

    public float aimRate { get; protected set; } = 60f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetForwardVelocity()
    {
        return _baseForwardVelocity;
    }

    public float GetReverseVelocity()
    {
        return _baseReverseVelocity;
    }

    public float GetStrafeVelocity()
    {
        return _baseStrafeVelocity;
    }

    public float GetRotationRate()
    {
        return _baseRotationRate;
    }

    public Vector3 GetJumpVector()
    {
        return jumpVector * jumpForce;
    }
}
