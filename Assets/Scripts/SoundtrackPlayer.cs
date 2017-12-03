using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundtrackPlayer : MonoBehaviour
{
    public static bool Playing;

    void Awake() 
    {
        if (Playing)
        {
            Destroy(gameObject);
        }

        Playing = true;

        DontDestroyOnLoad(gameObject);
    }

    void Start() 
    {
        
    }
    
    void Update() 
    {
        
    }
}
