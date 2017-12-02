using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour 
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

        if (transform.localPosition.x > 4.5f)
        {
            transform.localPosition -= 9f * Vector3.right;
        }

        if (transform.localPosition.x < -4.5f)
        {
            transform.localPosition += 9f * Vector3.right;
        }

        if (transform.localPosition.z > 8f)
        {
            transform.localPosition -= 16f * Vector3.forward;
        }

        if (transform.localPosition.z < -8f)
        {
            transform.localPosition += 16f * Vector3.forward;
        }
    }
}
