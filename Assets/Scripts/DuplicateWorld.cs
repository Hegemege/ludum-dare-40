using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DuplicateWorld : MonoBehaviour
{
    // If a master object exists, this object has no interaction or it's own will, it will copy all relevant properties from the Master
    [HideInInspector]
    public GameObject Master;
    public DuplicateWorld MasterScript;

    //protected List<GameObject> Mirrors;

    protected void Init()
    {
        //Mirrors = new List<GameObject>();
    }

    protected virtual void InitialSpawn()
    {
        
    }

    protected virtual void UpdateValues()
    {
        
    }
}
