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

    public float RequiredRatio;
    public int SpawnAmount;
    public bool RandomDirection;
    public float TotalSpawnTime;
    public int SpawnBursts;
    public float BurstInterval;
    public float SpawnMovementSpeed;

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
        
    }
}
