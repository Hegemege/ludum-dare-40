using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkerController : LimitToWorld
{
    public float MovementSpeed;
    public float TurningAngle;
    public float TurningSmoothing;

    [HideInInspector]
    public GameObject HomeBaseReference;
    [HideInInspector]
    public GameObject TargetReference;

    [HideInInspector]
    public bool RandomStartDirection;

    private Vector3 _targetDirection;

    private float _randomDirectionTimer;
    private float _randomDirectionTimerMax;
    private float _randomDirectionWeight;
    private float _randomSpeedWeight;

    private float _currentMovementSpeed;
    private float _movementSpeedTarget;

    private bool _spawned;

    void Awake()
    {
        _currentMovementSpeed = MovementSpeed;
    }

    /// <summary>
    /// Called from the spawner
    /// </summary>
    public void Init()
    {
        transform.localPosition = HomeBaseReference.transform.localPosition;

        if (RandomStartDirection)
        {
            transform.forward = Random.onUnitSphere;
            transform.forward = new Vector3(transform.forward.x, 0f, transform.forward.z);
            transform.forward.Normalize();
        }
        else
        {
            transform.forward = HomeBaseReference.transform.forward;
        }

        _targetDirection = transform.forward;

        ResetRandomDirectionTimer();
        _spawned = true;
    }
    
    void FixedUpdate()
    {
        if (!_spawned) return;

        var dt = Time.fixedDeltaTime;
        // Adjust direction slightly
        // Bias slightly towards _targetDirection

        var currentRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        var targetRotation = Quaternion.LookRotation(_targetDirection, Vector3.up);

        _randomDirectionTimer += dt;
        if (_randomDirectionTimer > _randomDirectionTimerMax)
        {
            ResetRandomDirectionTimer();
        }

        var angleChange = _randomDirectionWeight * TurningAngle;
        targetRotation *= Quaternion.AngleAxis(angleChange, Vector3.up);

        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, TurningSmoothing);

        // Move
        _currentMovementSpeed = Mathf.Lerp(_currentMovementSpeed, _movementSpeedTarget, 0.01f);

        transform.localPosition += transform.forward * _currentMovementSpeed * dt;

        base.FixedUpdate();
    }

    private void ResetRandomDirectionTimer()
    {
        _randomDirectionTimer = 0f;
        _randomDirectionTimerMax = Random.Range(0.25f, 0.5f);
        _randomDirectionWeight = Random.Range(-1f, 1f);
        _randomSpeedWeight = Random.Range(0.1f, 1f);
        _movementSpeedTarget = _randomSpeedWeight * MovementSpeed;
    }
}
