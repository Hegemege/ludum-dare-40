using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomeBaseController : MonoBehaviour
{
    public GameObject WalkerPrefab;
    public GameObject WalkerContainerRef;
    public GameObject TargetRef;

    public int SpawnAmount;
    public bool RandomStartDirection;
    public float TotalSpawnTime;
    public float SpawnMovementSpeed;

    private int _spawned;
    private float _spawnTimer;
    private float _spawnInterval;

    void Awake()
    {
        _spawnInterval = TotalSpawnTime / SpawnAmount;
    }

    void Start() 
    {
        
    }
    
    void Update()
    {
        var dt = Time.deltaTime;

        _spawnTimer += dt;

        if (_spawnTimer > _spawnInterval && _spawned < SpawnAmount)
        {
            Spawn();
            _spawned += 1;
            _spawnTimer = 0f;
        }
    }

    private void Spawn()
    {
        var walker = Instantiate(WalkerPrefab);
        walker.transform.parent = WalkerContainerRef.transform;

        var walkerScript = walker.GetComponent<WalkerController>();
        walkerScript.HomeBaseReference = gameObject;
        walkerScript.TargetReference = TargetRef;
        walkerScript.RandomStartDirection = RandomStartDirection;
        walkerScript.MovementSpeed *= SpawnMovementSpeed;
        walkerScript.Init();
    }
}
