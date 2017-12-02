using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DuplicateWorld : MonoBehaviour
{
    // If a master object exists, this object has no interaction or it's own will, it will copy all relevant properties from the Master
    [HideInInspector]
    public DuplicateWorld Master;

    protected List<GameObject> Mirrors;

    protected void Awake()
    {
        Mirrors = new List<GameObject>();
    }

    protected virtual void InitialSpawn()
    {
        
    }

    protected virtual void UpdateValues()
    {
        
    }
}
