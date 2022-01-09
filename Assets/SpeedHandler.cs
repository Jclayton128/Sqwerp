using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedHandler : MonoBehaviour
{
    //param
    [SerializeField] float _baseForwardVelocity = 1f;
    [SerializeField] float _baseReverseVelocity = 0.5f;
    [SerializeField] float _baseStrafeVelocity = 0.75f;

    [SerializeField] float aimRate = 90f;


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
}
