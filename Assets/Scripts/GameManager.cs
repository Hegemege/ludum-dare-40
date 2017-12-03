using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string NextScene;

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
    private float _levelClearedTimer;

    private AudioSource _collectAudio;

    public bool LevelLoadingDone;
    public bool SuppressEffectSounds;

    void Awake()
    {
        _collectAudio = GetComponent<AudioSource>();
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

        if (!LevelLoadingDone) return;

        if (Collected < RequiredAmount)
        {
            LevelTimer += dt;
        }
        else
        {
            Collected = RequiredAmount;
            LevelComplete();
        }

        if (LevelCleared)
        {
            _levelClearedTimer += dt;
            if (_levelClearedTimer > 3f)
            {
                _levelClearedTimer = 0f;
                SceneManager.LoadScene(NextScene);
            }
        }
    }

    private void LevelComplete()
    {
        LevelCleared = true;
        System.GC.Collect();
    }

    public void WalkerHitTarget()
    {
        if (GameManager.Instance.LevelCleared) return;
        if (!GameManager.Instance.SuppressEffectSounds)
        {
            _collectAudio.Play();
        }

        
        Collected += 1;
    }

    public void WalkerSpawned()
    {
        
    }
}
