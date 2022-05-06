using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ElectroMag._DevPooling
{
    public class ObjectPool : MonoBehaviour, IObjectPool
    {


        #region Properties
        public int NumObjectsToPool { get; set; }
        public GameObject ToPoolObject { get; set; }
        
        private List<GameObject> _pooledObjects;
        
        [Required]
        [HorizontalGroup("fab", LabelWidth = 100)]
        
        [VerticalGroup("fab/1")]
        [PreviewField]
        public GameObject poolPrefab;
        
        [VerticalGroup("fab/2")]
        [SerializeField] public int numberToPool;
        

        [BoxGroup("Pool Parameters")]
        public bool despawning;
        
        [BoxGroup("Pool Parameters")]
        [ShowIf("despawning")] [SerializeField]
        public int secondsToLive;
        #endregion
        
        private int _index = 0;

        public void Act(Vector3 pos)
        {
            if (_index >= _pooledObjects.Count) _index = 0;
            
            var go = _pooledObjects[_index];
                if (!go.activeInHierarchy)
                {
                    go.SetActive(true);
                    go.transform.position = pos;
                    
                    if (despawning)
                        StartCoroutine(TTL(secondsToLive, go));
                    _index++;
                    return;
                }
                HandleOutOfBounds(pos);
        }

        public void AddObject(GameObject prefab, int amtToAdd)
        {
            for(int i = 0; i < amtToAdd; i++)
                _pooledObjects.Add(prefab);
        }
        
        public void ApplyValues(int numPool, GameObject prefab)
        {
            //TODO DELET
            numberToPool = numPool;
            poolPrefab = prefab;
        }
        
        public void CreatePool(GameObject prefab)
        {
            for (int i = 0; i < numberToPool; i++)
            {
                var pooledObject = Instantiate(prefab, transform);
                _pooledObjects.Add(pooledObject);
                pooledObject.SetActive(false);
            }
        }

        private void HandleOutOfBounds(Vector3 pos)
        {
            var ob = Instantiate(ToPoolObject, transform);
            ob.transform.position = pos;
            _pooledObjects.Add(ob);
            if (despawning)
                StartCoroutine(TTL(secondsToLive, ob));
        }
        
        public void Initialize()
        {
            UpdateInspectorDefinedValues();
            _pooledObjects = new List<GameObject>();
            CreatePool(poolPrefab);
        }

        public void Sleep()
        {
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if(_pooledObjects[i].activeInHierarchy)
                    _pooledObjects[i].SetActive(false);
            }
        }


        private IEnumerator TTL(int sec, GameObject deactObj)
        {
            yield return new WaitForSeconds(sec);
            deactObj.SetActive(false);
        }
        
        void UpdateInspectorDefinedValues()
        {
            NumObjectsToPool = numberToPool;
            ToPoolObject = poolPrefab;
        }
    }
}

