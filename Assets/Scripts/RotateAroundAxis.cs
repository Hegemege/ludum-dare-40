using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotateAroundAxis : MonoBehaviour
{
    public Vector3 Axis;
    public float RotationSpeed;

    private Quaternion _startRotation;
    private float _rotationAngle;

    void Awake()
    {
        _startRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;

        _rotationAngle += RotationSpeed * dt;

        transform.localRotation = _startRotation * Quaternion.AngleAxis(_rotationAngle, Vector3.up);
    }
}
