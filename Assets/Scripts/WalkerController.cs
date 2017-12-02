using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkerController : MonoBehaviour
{
    public float MovementSpeed;
    public float TurningAngle;
    public float TurningSmoothing;

    [HideInInspector]
    public GameObject HomeBaseReference;
    [HideInInspector]
    public GameObject TargetReference;

    private Vector3 _targetDirection;

    private float _randomDirectionTimer;
    private float _randomDirectionTimerMax;
    private float _randomDirectionWeight;

    void Awake()
    {
        transform.forward = Random.onUnitSphere;
        transform.forward = new Vector3(transform.forward.x, 0f, transform.forward.z);
        transform.forward.Normalize();

        _targetDirection = transform.forward;

        Debug.DrawRay(transform.position, _targetDirection, Color.red, 5f);

        // Initialize random direction change timer;
        ResetRandomDirectionTimer();
    }

    /// <summary>
    /// Called from the spawner
    /// </summary>
    void Init()
    {
        transform.localPosition = HomeBaseReference.transform.localPosition;
    }
    
    void FixedUpdate()
    {
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
        var effectiveMovementSpeed = MovementSpeed;

        transform.localPosition += transform.forward * effectiveMovementSpeed * dt;
    }

    private void ResetRandomDirectionTimer()
    {
        _randomDirectionTimer = 0f;
        _randomDirectionTimerMax = Random.Range(0.25f, 0.5f);
        _randomDirectionWeight = Random.Range(-1f, 1f);
    }
}
