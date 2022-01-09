using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpeedHandler), typeof(InputHandler))]
public class HeadMovementHandler : MonoBehaviour
{
    SpeedHandler sh;
    InputHandler ih;

    // Start is called before the first frame update
    void Start()
    {
        sh = GetComponentInParent<SpeedHandler>();
        ih = GetComponentInParent<InputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(ih.aimPos);
    }
}
