using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkerPool : MonoBehaviour
{
    public GameObject PooledObjectPrefab;
    public int PooledAmount;
    public bool WillGrow;

    public List<GameObject> Pool;

    private GameObject _walkerPoolContainer;

    void Awake() 
    {
        _walkerPoolContainer = new GameObject();
        _walkerPoolContainer.name = "WalkerPool";

        Pool = new List<GameObject>();
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < Pool.Count; i++)
        {
            if (Pool[i] == null)
            {
                GameObject obj = (GameObject)Instantiate(PooledObjectPrefab);
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
            Pool.Add(obj);
            return obj;
        }

        return null;
    }
}
