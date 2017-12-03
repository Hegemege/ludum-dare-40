using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomeBaseController : MonoBehaviour
{
    public GameObject WalkerPrefab;
    [HideInInspector]
    public GameObject TargetRef;

    [HideInInspector]
    public int SpawnAmount;
    [HideInInspector]
    public bool RandomStartDirection;
    [HideInInspector]
    public float TotalSpawnTime;
    [HideInInspector]
    public int SpawnBursts;
    [HideInInspector]
    public float BurstInterval;
    [HideInInspector]
    public float SpawnMovementSpeed;

    private int _burstsDone;
    private float _burstTimer;
    private int _spawned;
    private float _spawnTimer;
    private float _spawnInterval;
    private GameObject _walkerContainerRef;

    void Awake()
    {
        _spawnInterval = TotalSpawnTime / SpawnAmount;
        _walkerContainerRef = new GameObject();
        _walkerContainerRef.name = "WalkerContainer";

        _burstTimer = 0f;
        _burstsDone = 0;
    }

    void Start() 
    {
        
    }
    
    void Update()
    {
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
            if (_burstTimer > BurstInterval)
            {
                _burstsDone += 1;
                _burstTimer = 0f;
            }
        }
    }

    private void Spawn()
    {
        var walker = Instantiate(WalkerPrefab);
        walker.transform.parent = _walkerContainerRef.transform;

        var walkerScript = walker.GetComponent<WalkerController>();
        walkerScript.HomeBaseReference = gameObject;
        walkerScript.TargetReference = TargetRef;
        walkerScript.RandomStartDirection = RandomStartDirection;
        walkerScript.MovementSpeed *= SpawnMovementSpeed;
        walkerScript.Init();
    }
}
