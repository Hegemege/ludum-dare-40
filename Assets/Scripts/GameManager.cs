using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    //Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject HomeBaseRef;
    public GameObject TargetRef;

    public int ArrowStorage;
    public int RequiredAmount;
    public int SpawnAmount;
    public bool RandomDirection;
    public float TotalSpawnTime;
    public int SpawnBursts;
    public float BurstInterval;
    public float SpawnMovementSpeed;

    public int MaximumWalkers;
    [HideInInspector]
    public int AliveWalkers;

    [HideInInspector]
    public float LevelTimer;

    [HideInInspector]
    public int Collected;

    [HideInInspector]
    public bool LevelCleared;

    void Awake() 
    {
        _instance = this;

        var homeBaseScript = HomeBaseRef.GetComponent<HomeBaseController>();
        homeBaseScript.TargetRef = TargetRef;
        homeBaseScript.SpawnAmount = SpawnAmount;
        homeBaseScript.RandomStartDirection = RandomDirection;
        homeBaseScript.TotalSpawnTime = TotalSpawnTime;
        homeBaseScript.SpawnBursts = SpawnBursts;
        homeBaseScript.BurstInterval = BurstInterval;
        homeBaseScript.SpawnMovementSpeed = SpawnMovementSpeed;
    }

    void Start() 
    {
        
    }
    
    void Update()
    {
        var dt = Time.deltaTime;

        if (Collected < RequiredAmount)
        {
            LevelTimer += dt;
        }
        else
        {
            Collected = RequiredAmount;
            LevelComplete();
        }
    }

    private void LevelComplete()
    {
        LevelCleared = true;
    }

    public void WalkerHitTarget()
    {
        Collected += 1;
    }

    public void WalkerSpawned()
    {
        
    }
}
