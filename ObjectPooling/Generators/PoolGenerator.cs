using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectPooling
{
    public class PoolGenerator : MonoBehaviour, IGenerator
    {
        #region Properties
        public List<ObjectPool> currentPools;
        public string generatorName { get; set; }
        private Dictionary<string, IObjectPool> _poolDictionary;
        #endregion
        #region InspectorTools

        [HorizontalGroup("New Generator", 0.5f, LabelWidth = 100)]
        [BoxGroup("New Generator/Generator Info")] public string poolName;
        [BoxGroup("New Generator/Generator Info")] public int amountToPool;
        
        [BoxGroup("New Generator/Generator Info")] public bool despawn;
        [ShowIf("despawn")]public int lifetimeSeconds;
        
        [Space] [BoxGroup("New Generator/Generator Prefab")]
        [PreviewField] public GameObject prefabToPool;
        
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
        public void AddObjectPool()
        {
            GameObject go = new GameObject(poolName);
            IObjectPool op = go.AddComponent<ObjectPool>();
            op.ApplyValues(amountToPool, prefabToPool);
            
            ValidateName(go);
            
            AddNewChildToPools(op, go);
            
            ResetInspectorPoolList();
            ResetInspectorProperties();
        }

        private void AddNewChildToPools(IObjectPool op, GameObject go)
        {
            if (go == null) return;
            go.GetComponent<ObjectPool>().despawning = despawn;
            go.GetComponent<ObjectPool>().secondsToLive = lifetimeSeconds;
            currentPools.Add((ObjectPool) op);
            go.transform.parent = transform;
        }

        void ResetInspectorPoolList()
        {
            currentPools.Clear();
            foreach (Transform child in transform)
            {
                if(child.GetComponent(typeof(IObjectPool)) != null)
                    currentPools.Add(child.GetComponent<ObjectPool>());
            }
        }
        
        private void ResetInspectorProperties()
        {
            poolName = "";
            prefabToPool = null;
        }

        public void UpdateInspectorValues()
        {
            generatorName = gameObject.name;
        }
        
        private void ValidateName(GameObject go)
        {
            foreach (Transform child in transform)
            {
                if (child.name == poolName)
                {
                    DestroyImmediate(go);
                    poolName = "Error";
                }
            }
        }

        #endregion

        public IObjectPool GetPool(string str)
        {
            return _poolDictionary[str];
        }
        
        public int GetPoolsCount()
        {
            return _poolDictionary.Count;
        }
        
        public void Initialize()
        {
            UpdateInspectorValues();
            InitializeOwnedObjectPools();
        }

        private void InitializeOwnedObjectPools()
        {
            _poolDictionary = new Dictionary<string, IObjectPool>();
            
            foreach (Transform child in transform)
            {
                IObjectPool obj = (IObjectPool) child.GetComponent((typeof(IObjectPool)));
                obj.Initialize();
                _poolDictionary.Add(child.gameObject.name, obj); ;
            }
        }

        public void SleepPools()
        {
            foreach (var pool in _poolDictionary)
                pool.Value.Sleep();
        }
    }
}
