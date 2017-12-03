using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeathParticlePool : ParticleSystemPool
{
    //Singleton
    private static DeathParticlePool _instance;
    public static DeathParticlePool Instance
    {
        get
        {
            return _instance;
        }
    }

    protected override void Awake() 
    {
        base.Awake();

        _instance = this;
    }

    void Start() 
    {
        
    }
    
    void Update() 
    {
        
    }
}
