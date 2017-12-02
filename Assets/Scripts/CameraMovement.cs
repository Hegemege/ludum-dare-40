using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMovement : LimitToWorld
{
    void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;

        dt *= 5f;

        if (Input.GetKey("a"))
        {
            transform.localPosition += new Vector3(0f, 0f, -1f) * dt;
        }
        if (Input.GetKey("w"))
        {
            transform.localPosition += new Vector3(-1f, 0f, 0f) * dt;
        }
        if (Input.GetKey("s"))
        {
            transform.localPosition += new Vector3(1f, 0f, 0f) * dt;
        }
        if (Input.GetKey("d"))
        {
            transform.localPosition += new Vector3(0f, 0f, 1f) * dt;
        }

        base.FixedUpdate();
    }
}
