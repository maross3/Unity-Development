using System.Collections;
using System.Collections.Generic;
using ElectroMag._DevPooling;
using Sirenix.OdinInspector;
using UnityEngine;

public class TreasurePool : MonoBehaviour, IObjectPool
{
    public int amountToPool;
    [PreviewField]
    public GameObject poolPrefab;
    public bool despawning;
    public int secondsToLive;
    
    private int index;
    private GameObject go;
    private List<GameObject> _pooledObjects;
    public void ApplyValues(int numPool, GameObject prefab)
    {
        
    }

    public int NumObjectsToPool { get; set; }
    public GameObject ToPoolObject { get; set; }
    public void Initialize()
    {
        _pooledObjects = new List<GameObject>();
        CreatePool(poolPrefab);
        index = 0;
    }

    public void Act(Vector3 pos)
    {
        if (index >= _pooledObjects.Count) index = 0;
        if (!_pooledObjects[index].activeInHierarchy)
        {
            go = _pooledObjects[index];
            go.SetActive(true);
            go.transform.position = pos;

            if (despawning)
                StartCoroutine(TTL(secondsToLive, go));
            
            index++;
            return;
            
        }
        HandleOutOfBounds(pos);
    }

    public void Sleep()
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if(_pooledObjects[i].activeInHierarchy)
                _pooledObjects[i].SetActive(false);
        }
    }

    public void CreatePool(GameObject prefab)
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject pooledObj = Instantiate(prefab, transform);
            _pooledObjects.Add(pooledObj);
            pooledObj.SetActive(false);
        }
    }

    public void AddObject(GameObject prefab, int amtToAdd)
    {
        for(int i = 0; i < amtToAdd; i++)
            _pooledObjects.Add(prefab);
    }
    
    private void HandleOutOfBounds(Vector3 pos)
    {
        go = Instantiate(poolPrefab, transform);
        go.transform.position = pos;
        _pooledObjects.Add(go);
        if (despawning)
            StartCoroutine(TTL(secondsToLive, go));
    }
    
    private IEnumerator TTL(int sec, GameObject deactObj)
    {
        yield return new WaitForSeconds(sec);
        deactObj.SetActive(false);
    }
}
