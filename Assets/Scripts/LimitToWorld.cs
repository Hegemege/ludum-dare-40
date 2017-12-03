using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LimitToWorld : MonoBehaviour 
{
    void Awake() 
    {
        
    }

    void Start() 
    {
        
    }
    
    protected virtual void FixedUpdate() 
    {
        if (transform.localPosition.x > 4.5f)
        {
            transform.localPosition -= 9f * Vector3.right;
        }

        if (transform.localPosition.x < -4.5f)
        {
            transform.localPosition += 9f * Vector3.right;
        }

        if (transform.localPosition.z > 8f)
        {
            transform.localPosition -= 16f * Vector3.forward;
        }

        if (transform.localPosition.z < -8f)
        {
            transform.localPosition += 16f * Vector3.forward;
        }
    }
}
