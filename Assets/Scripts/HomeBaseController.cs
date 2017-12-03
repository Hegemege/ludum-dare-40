using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomeBaseController : MonoBehaviour
{
    public GameObject WalkerPrefab;
    [HideInInspector]
    public GameObject TargetRef;

    public int SpawnAmount;
    public bool RandomStartDirection;
    public float TotalSpawnTime;
    public int SpawnBursts;
    public float BurstInterval;
    public float SpawnMovementSpeed;
    public bool EnforceWorldLimit = true;

    private int _burstsDone;
    private float _burstTimer;
    private int _spawned;
    private float _spawnTimer;
    private float _spawnInterval;
    private GameObject _walkerContainerRef;

    public float MaxRotationSpeed;
    private Random3DRotation _beaconRotator;

    private WalkerPool _walkerPool;

    void Awake()
    {
        _spawnInterval = TotalSpawnTime / SpawnAmount;
        _walkerContainerRef = new GameObject();
        _walkerContainerRef.name = "WalkerContainer";
        _beaconRotator = GetComponentInChildren<Random3DRotation>();

        _burstTimer = 0f;
        _burstsDone = 0;

        _walkerPool = GetComponent<WalkerPool>();
    }

    void Start() 
    {
        
    }
    
    void Update()
    {
        if (GameManager.Instance.LevelCleared) return;
        if (!GameManager.Instance.LevelLoadingDone) return;

        if (GameManager.Instance.AliveWalkers >= GameManager.Instance.MaximumWalkers) return;

        var dt = Time.deltaTime;

        _spawnTimer += dt;

        if (_spawnTimer > _spawnInterval && _spawned < SpawnAmount * _burstsDone)
        {
            Spawn();
            _spawned += 1;
            _spawnTimer = 0f;
        }

        // If the burst is done, check if we can do more bursts
        if (_spawned >= SpawnAmount * _burstsDone && _burstsDone < SpawnBursts)
        {
            _burstTimer += dt;
            _beaconRotator.RotationSpeed = Mathf.Lerp(0f, MaxRotationSpeed, _burstTimer / BurstInterval);

            if (_burstTimer > BurstInterval)
            {
                _burstsDone += 1;
                _burstTimer = 0f;
            }
        }
        else
        {
            _beaconRotator.RotationSpeed *= 0.95f;
        }
    }

    private void Spawn()
    {
        var walker = _walkerPool.GetPooledObject();// Instantiate(WalkerPrefab);
        if (walker == null)
        {
            return;
        }

        walker.transform.parent = _walkerContainerRef.transform;
        if (!walker.activeInHierarchy && walker.transform.localPosition.y > -2f)
        {
            walker.SetActive(true);
        }
        else
        {
            walker.SetActive(true);

            var walkerScript = walker.GetComponent<WalkerController>();
            walkerScript.Limit = EnforceWorldLimit;
            walkerScript.HomeBaseReference = gameObject;
            walkerScript.TargetReference = TargetRef;
            walkerScript.RandomStartDirection = RandomStartDirection;
            walkerScript.MovementSpeed *= SpawnMovementSpeed;
            walkerScript.Init();
        }
    }
}
