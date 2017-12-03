using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetParticlePool : ParticleSystemPool 
{
    //Singleton
    private static TargetParticlePool _instance;
    public static TargetParticlePool Instance
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
