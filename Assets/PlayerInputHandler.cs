using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : InputHandler 
{
    [SerializeField] GameObject aimpoint = null;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        ListenForKeyboardInput();
        if (Input.GetKey(KeyCode.Mouse0))
        {
            ListenForMouseInput();
        }

    }


    private void ListenForMouseInput()
    {
        Vector2 mousePos = Input.mousePosition;
        Camera cam = Camera.main;
        Vector3 pos = new Vector3(mousePos.x, mousePos.y, cam.farClipPlane);
        aimPos = Camera.main.ScreenToWorldPoint(pos) - transform.position;

        aimpoint.transform.position = aimPos;
        Debug.DrawLine(transform.position, aimPos * 1, Color.red);
    }   

    private void ListenForKeyboardInput()
    {
        CommandedForwardSignal = GetForwardSignal();
        CommandedYawSignal = GetYawSignal();
        JumpRequested = Input.GetKeyUp(KeyCode.Space);
    }

    private float GetYawSignal()
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
