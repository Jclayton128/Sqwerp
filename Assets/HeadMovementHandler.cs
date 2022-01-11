using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpeedHandler), typeof(InputHandler))]
public class HeadMovementHandler : MonoBehaviour
{
    SpeedHandler sh;
    InputHandler ih;
    BodyMovementHandler bmh;
    [SerializeField] GameObject gyro = null;

    //param
    //param
    float aimNullZone = 10f;
    [SerializeField] float yawToTurn = 45f;
    float yawGimbal = 60;
    float pitchGimbal = 20f;

    //state
    float currentYaw;
    public float CommandedPitchSignal; //{ get; protected set; }
    public float CommandedYawSignal; // { get; protected set; }

    Vector3 updateRotation = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        sh = GetComponentInParent<SpeedHandler>();
        ih = GetComponentInParent<InputHandler>();
        bmh = GetComponentInParent<BodyMovementHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        gyro.transform.LookAt(ih.aimPos);

        currentYaw = transform.localEulerAngles.y;
        if (currentYaw > 180f)
        {
            currentYaw -= 360f;
        }

        CommandedPitchSignal = SetPitchSignal();
        CommandedYawSignal = SetYawSignal();

        TurnHead();

        TransmitBodyTurnSignals();
    }



    private float SetYawSignal()
    {
        float amount = gyro.transform.localRotation.eulerAngles.y;
        if (amount > 180)
        {
            amount -= 360f;
        }
        return amount;
    }

    private float SetPitchSignal()
    {
        float amount = gyro.transform.localRotation.eulerAngles.x;
        if (amount > 180)
        {
            amount -= 360f;
        }
        return amount;

    }

    private void TurnHead()
    {
        AdjustYaw();
        AdjustPitch();

        transform.localEulerAngles = updateRotation;
    }

    private void AdjustPitch()
    {
        if (Mathf.Abs(CommandedPitchSignal) >= aimNullZone)
        {
            updateRotation.x = transform.localRotation.eulerAngles.x +
                (Mathf.Sign(CommandedPitchSignal) * sh.aimRate * Time.deltaTime);

            if (updateRotation.x > pitchGimbal && updateRotation.x < 180f)
            {
                Debug.Log($"should limit downward pitch");
                updateRotation.x = pitchGimbal - 1;
            }
            if (updateRotation.x > 180f && updateRotation.x < (360f - pitchGimbal))
            {
                Debug.Log($"should limit upward pitch");
                updateRotation.x = (360 - pitchGimbal + 1f);
            }



            //if (updateRotation.y < (360 - yawGimbal) && updateRotation.y > yawGimbal)
            //{
            //    updateRotation.y = (360 - yawGimbal + 1);
            //}
        }
    }

    private void AdjustYaw()
    {
        if (Mathf.Abs(CommandedYawSignal) >= aimNullZone)
        {
            if (currentYaw <= yawGimbal && currentYaw >= 0)
            {
                updateRotation.y = transform.localRotation.eulerAngles.y +
                    (Mathf.Sign(CommandedYawSignal) * sh.aimRate * Time.deltaTime);
                if (updateRotation.y > yawGimbal)
                {
                    updateRotation.y = yawGimbal -1;
                }
                return;
            }
            if (currentYaw >= -1 * yawGimbal && currentYaw <= 0)
            {
                updateRotation.y = transform.localRotation.eulerAngles.y +
                    (Mathf.Sign(CommandedYawSignal) * sh.aimRate * Time.deltaTime);

                if (updateRotation.y < (360-yawGimbal) && updateRotation.y > yawGimbal)
                {
                    updateRotation.y = (360 - yawGimbal +1);
                }
                return;
            }

        }

    }

    private void TransmitBodyTurnSignals()
    {
        if (Mathf.Abs(currentYaw) > yawToTurn)
        {
            bmh.SetRequestedYaw(Mathf.Sign(currentYaw));
        }
        else
        {
            bmh.SetRequestedYaw(0);
        }
    }
}
