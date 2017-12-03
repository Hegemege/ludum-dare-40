using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMovement : LimitToWorld
{
    private Camera _cameraRef;

    void Awake()
    {
        _cameraRef = GetComponent<Camera>();
    }

    void Update()
    {
        // Force camera aspect ratio to 16:9
        float targetaspect = 16.0f / 9.0f;
        float windowAspect = (float)Screen.width / (float)Screen.height;

        float scaleHeight = windowAspect / targetaspect;

        // if scaled height is less than current height, add letterbox
        if (scaleHeight < 1.0f)
        {
            Rect rect = _cameraRef.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            _cameraRef.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleHeight;

            Rect rect = _cameraRef.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            _cameraRef.rect = rect;
        }
    }

    protected override void FixedUpdate()
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
