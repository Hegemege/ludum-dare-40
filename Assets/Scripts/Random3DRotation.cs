using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Random3DRotation : MonoBehaviour
{
    public float RotationSpeed;

    private Vector3 _randomRotation;
    private Vector3 _newRandomRotation;

    public float RotationChangeInterval;
    private float _rotationChangeTimer;

    void Awake()
    {
        _randomRotation = Random.onUnitSphere;
        _newRandomRotation = Random.onUnitSphere;
    }

    void Start() 
    {
        
    }
    
    void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;

        _rotationChangeTimer += dt;
        if (_rotationChangeTimer > RotationChangeInterval)
        {
            _randomRotation = _newRandomRotation;
            _newRandomRotation = Random.onUnitSphere;
            _rotationChangeTimer = 0f;
        }

        var effectiveRotation = Quaternion.Slerp(Quaternion.Euler(_randomRotation * RotationSpeed * dt), Quaternion.Euler(_newRandomRotation * RotationSpeed * dt), _rotationChangeTimer / RotationChangeInterval);

        transform.localRotation *= effectiveRotation;
    }
}
