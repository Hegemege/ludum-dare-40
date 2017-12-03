using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleSystemPool : MonoBehaviour 
{
    public GameObject PooledObjectPrefab;
    public bool WillGrow;

    public List<GameObject> Pool;

    private GameObject _poolContainer;

    protected virtual void Awake()
    {
        _poolContainer = new GameObject();
        _poolContainer.name = "ParticleSystemPool";

        Pool = new List<GameObject>();
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < Pool.Count; i++)
        {
            if (Pool[i] == null)
            {
                GameObject obj = (GameObject)Instantiate(PooledObjectPrefab);
                obj.transform.parent = _poolContainer.transform;
                obj.SetActive(false);
                obj.transform.localPosition = Vector3.up * -3f;
                Pool[i] = obj;
                return Pool[i];
            }
            if (!Pool[i].activeInHierarchy)
            {
                return Pool[i];
            }
        }

        if (WillGrow)
        {
            GameObject obj = (GameObject)Instantiate(PooledObjectPrefab);
            obj.transform.parent = _poolContainer.transform;
            Pool.Add(obj);
            return obj;
        }

        return null;
    }
}
