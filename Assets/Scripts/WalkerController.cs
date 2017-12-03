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

    public LayerMask WalkerObstacleLayer;

    private Vector3 _targetDirection;

    private float _randomDirectionTimer;
    private float _randomDirectionTimerMax;
    private float _randomDirectionWeight;
    private float _randomSpeedWeight;

    private float _currentMovementSpeed;
    private float _movementSpeedTarget;

    private bool _spawned;
    private bool _freeze;
    private bool _reinitialize;

    public List<AudioClip> BurnAudioClips;
    private AudioSource _burnAudioSource;

    void Awake()
    {
        _burnAudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Called from the spawner
    /// </summary>
    public void Init()
    {
        _currentMovementSpeed = MovementSpeed;
        GameManager.Instance.AliveWalkers += 1;

        transform.localPosition = HomeBaseReference.transform.localPosition;
        transform.localPosition = new Vector3(transform.localPosition.x, 0.01f, transform.localPosition.z);

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
        _reinitialize = false;
        _freeze = false;
    }
    
    protected override void FixedUpdate()
    {
        if (_reinitialize && !_freeze)
        {
            Init();
            return;
        }

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

        // Spherecast forward, if there is something, turn back
        Ray forwardRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(forwardRay, 0.1f, out hit, _currentMovementSpeed * dt, WalkerObstacleLayer))
        {
            var newForwad = Vector3.Reflect(transform.forward, hit.normal);
            newForwad.y = 0f;
            newForwad.Normalize();
            transform.rotation = Quaternion.LookRotation(newForwad, Vector3.up);
            _targetDirection = transform.forward;
        }

        transform.localPosition += transform.forward * _currentMovementSpeed * dt;

        // TODO: if stuck in object, kill

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

    void OnTriggerEnter(Collider other)
    {
        if (_reinitialize) return;

        if (other.CompareTag("DirectionArrow"))
        {
            // Realign
            _targetDirection = other.transform.parent.forward;
        }
        else if (other.CompareTag("LaserBeam"))
        {
            // Die
            var particles = DeathParticlePool.Instance.GetPooledObject(); //Instantiate(DeathParticles);
            particles.SetActive(true);
            particles.transform.position = transform.position;

            GameManager.Instance.AliveWalkers -= 1;

            // Burn audio
            if (!GameManager.Instance.LevelCleared && !GameManager.Instance.SuppressEffectSounds)
            {
                var randomClip = BurnAudioClips[Random.Range(0, BurnAudioClips.Count)];
                _burnAudioSource.clip = randomClip;
                _burnAudioSource.Play();
            }

            ReturnToPool();
        }
        else if (other.CompareTag("Target"))
        {
            // Spawn particles and destroy, inform game manager
            var particles = TargetParticlePool.Instance.GetPooledObject(); //Instantiate(TargetParticles);
            particles.SetActive(true);
            particles.transform.position = transform.position + Vector3.up * 0.1f;

            GameManager.Instance.WalkerHitTarget();

            GameManager.Instance.AliveWalkers -= 1;

            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        _reinitialize = true;
        transform.position = Vector3.up * -1f;
        _freeze = true;
        StartCoroutine(SetInactive());
    }

    private IEnumerator SetInactive()
    {
        yield return new WaitForSeconds(1f);
        _freeze = false;
        gameObject.SetActive(false);
    }
}
