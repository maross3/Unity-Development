using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

//Summary:
    // Responsible for intializting and fetching objects from the pool
    // to use:
    // PoolMaster.SpawnObjectAt("Fish", "Bass", hook.transform.position);

namespace ElectroMag._DevPooling
{
    public class PoolMaster : MonoBehaviour
    {
        public static PoolMaster SharedInstance;
        private Dictionary<string, IGenerator> _generatorDictionary;

        #region  InspectorTools

        public enum GeneratorType
        {
            PoolGenerator,
            TreasureGenerator
        }

        public GeneratorType generatorType;
        
        [BoxGroup("New Pool Generator")] public string generatorName;
        
        [ToggleLeft]
        [BoxGroup("New Pool Generator")] public bool dontDestroyOnLoad;
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
        public void CreateNewGenerator()
        {
            GameObject go = new GameObject(generatorName);

            if(generatorType == GeneratorType.PoolGenerator)
                go.AddComponent<PoolGenerator>();
            else if (generatorType == GeneratorType.TreasureGenerator)
            {
                TreasureGenerator tregen = go.AddComponent<TreasureGenerator>();
                CreateScriptableObject(tregen);
            }
            
            
            go.transform.parent = transform;
            generatorName = "";
        }

        private void CreateScriptableObject(TreasureGenerator gen)
        {
            LootTier tier = ScriptableObject.CreateInstance<LootTier>();
            gen.tier = tier;
            
            // tiers and treasure folders
            CheckCreateFolder("Assets/Scripts/ElectroMag/_DevPooling/TreasureSO", "Tiers");
            CheckCreateFolder("Assets/Scripts/ElectroMag/_DevPooling/TreasureSO", "Treasures");
            
            // Making Generator Scriptable Object
            string path = "Assets/Scripts/ElectroMag/_DevPooling/TreasureSO/Tiers/" + generatorName + ".asset";
            AssetDatabase.CreateAsset(tier, path);
            AssetDatabase.Refresh();
            
            // Make treasures folder for generator
            string folderPath = "Assets/Scripts/ElectroMag/_DevPooling/TreasureSO/Treasures";
            AssetDatabase.CreateFolder(folderPath, generatorName);

        }

        private static void CheckCreateFolder(string pth, string fldr)
        {
            if (!AssetDatabase.IsValidFolder(pth + "/" + fldr))
                AssetDatabase.CreateFolder(pth, fldr);
        }
        
        [Button(ButtonSizes.Large), GUIColor(1, 0, 0)]
        public void CleanUp()
        {
            AssetDatabase.MoveAsset("Assets/Scripts/ElectroMag/_DevPooling/TreasureSO/Treasures",
                "Assets/Scripts/ElectroMag/_DevPooling/TreasureSO/Temp");
            
            List<TierItem> items = new List<TierItem>();

            foreach (Transform gen in transform)
            {
                if (gen.GetComponent<TreasureGenerator>() == null) continue;
                var itemsList = gen.GetComponent<TreasureGenerator>().tierItemsList;
                foreach (var item in itemsList)
                {
                    Debug.Log(item.itemName);
                    var newItem = ScriptableObject.CreateInstance<TierItem>();
                    newItem = item;
                    Debug.Log(newItem.itemName);
                    items.Add(newItem);
                }
                
                AssetDatabase.DeleteAsset("Assets/Scripts/ElectroMag/_DevPooling/TreasureSO/Treasures/" + gen.name);
                Debug.Log(items[0].itemName);
                
                AssetDatabase.CreateFolder("Assets/Scripts/ElectroMag/_DevPooling/TreasureSO/Treasures", gen.name);
                
                foreach(var tierItem in itemsList)
                {
                    var newTierItem = ScriptableObject.CreateInstance<TierItem>();
                    newTierItem.itemName = tierItem.itemName;
                    
                    string path = "Assets/Scripts/ElectroMag/_DevPooling/TreasureSO/Treasures/" + gen.gameObject.name + "/" + tierItem.itemName + ".asset";
                    Debug.Log(newTierItem);
                    AssetDatabase.CreateAsset(newTierItem, path);
                }
            }

        }
        #endregion
        
        private void Awake()
        {
            if(dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
            
            SharedInstance = this;
            InitializeGeneratorList();
        }

        private void InitializeGeneratorList()
        {
            _generatorDictionary = new Dictionary<string, IGenerator>();
            foreach (Transform child in transform)
            {
                IGenerator gen = (IGenerator) child.GetComponent(typeof(IGenerator));
                gen.UpdateInspectorValues();
                try
                {
                    _generatorDictionary.Add(gen.generatorName, gen);
                    gen.Initialize();
                }
                catch
                {
                    CatchDictionaryException(gen);
                }
            }
        }
        
        private void CatchDictionaryException(IGenerator gen)
        {
            _generatorDictionary.Add("ERROR", gen);
            gen.Initialize();
            Debug.Log("duplicate name " + gen.generatorName + "! You should probably fix that.");
        }

        #region StaticMethods
        public static void SleepAll()
        {
            foreach (var gen in SharedInstance._generatorDictionary)
                gen.Value.SleepPools();
        }
        
        public static void SpawnObjectAt(string gen, string objName, Vector3 pos)
        {
            IObjectPool pool = SharedInstance._generatorDictionary[gen].GetPool(objName);
            pool.Act(pos);
        }

        public static void RollForLoot(string tier, Vector3 pos)
        {
           IObjectPool loot = SharedInstance._generatorDictionary[tier].GetPool(tier);
           loot.Act(pos);
        }
        #endregion
    }
}
