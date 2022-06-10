using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ObjectPooling
{
    public class TreasureGenerator : MonoBehaviour, IGenerator
    {
        private Dictionary<string, TierItem> tierItemsDictionary;
        private Dictionary<string, IObjectPool> poolTierDictionary;
        public LootTier tier;

        #region InspectorVars

        void RollUpdate()
        {
            tier.debugRollsToCount = debugRolls;
        }

        [OnValueChanged("RollUpdate")] public int debugRolls;
        [Space] public List<TierItem> tierItemsList;

        [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
        void AddTreasurePool()
        {
            GameObject go = new GameObject(treasureName);
            ValidateName(go);

            if (treasureName == "Error") return;

            TreasurePool tres = go.AddComponent<TreasurePool>();
            tres.amountToPool = amountToPool;
            tres.poolPrefab = tierItemPrefab;

            if (despawn)
            {
                tres.despawning = true;
                tres.secondsToLive = seconds;
            }

            go.transform.parent = transform;

            CreateScriptableObject();
            ResetTreasureVariables();
        }

        private void ResetTreasureVariables()
        {
            amountToPool = 0;
            tierItemPrefab = null;
            dropModifier = 0;
            treasureName = "";
            values = null;
            tier.itemsInTier = tierItemsList;
        }

        [Button]
        void QuickRoll()
        {
            ClearConsole();
            tier.RollForTierLootDebug();
        }

        public void ClearConsole()
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }

        private void ValidateName(GameObject go)
        {
            foreach (Transform child in transform)
            {
                if (child.name == treasureName)
                {
                    DestroyImmediate(go);
                    treasureName = "Error";
                }
            }
        }

        [Required] [HorizontalGroup("fab", LabelWidth = 100)] [PreviewField] [VerticalGroup("fab/1")]
        public GameObject tierItemPrefab;

        [VerticalGroup("fab/2")] public int amountToPool;

        [VerticalGroup("fab/2")] public int dropModifier;

        [ToggleLeft] [VerticalGroup("fab/2")] public bool despawn;

        [ShowIf("despawn")] [VerticalGroup("fab/2")]
        public int seconds;

        [Space] [HorizontalGroup("New Treasure")]
        public string treasureName;

        public enum CharacterAttributes
        {
            Defense = 0,
            Strength = 1,
            Stamina = 2
        }

        [Serializable]
        public struct ItemValues
        {
            [HorizontalGroup("Split", 0.6f, LabelWidth = 50)]
            public CharacterAttributes identifier;

            [HorizontalGroup("Split", 0.2f)] public int value;
        }

        [Space] public ItemValues[] values;


        private void CreateScriptableObject()
        {
            TierItem item = ScriptableObject.CreateInstance<TierItem>();
            item.icon = GeneratePreviewIcon();
            item.itemName = treasureName;
            item.tierItemPrefab = tierItemPrefab;
            item.values = values;
            item.dropModifier = dropModifier;

            string path = "Assets/Scripts/ElectroMag/_DevPooling/TreasureSO/Treasures/" + gameObject.name + "/" +
                          treasureName + ".asset";
            AssetDatabase.CreateAsset(item, path);
            AssetDatabase.Refresh();
            // inspector/production purpose
            tierItemsList.Add(item);
        }



        private Sprite GeneratePreviewIcon()
        {
            var preview = AssetPreview.GetAssetPreview(tierItemPrefab);

            if (!AssetDatabase.IsValidFolder("Assets/Scripts/ElectroMag/_DevPooling/TreasureSO/Icons"))
                AssetDatabase.CreateFolder("Assets/Scripts/ElectroMag/_DevPooling/TreasureSO", "Icons");

            byte[] sprBytes = preview.EncodeToPNG();
            string path = "Assets/Scripts/ElectroMag/_DevPooling/TreasureSO/Icons/" + treasureName + ".png";
            File.WriteAllBytes(path, sprBytes);
            AssetDatabase.Refresh();

            var spr2 = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            return spr2;
        }

        #endregion
        
        public void Initialize()
        {
            tier.itemsInTier = tierItemsList;
            tierItemsDictionary = new Dictionary<string, TierItem>();
            poolTierDictionary = new Dictionary<string, IObjectPool>();

            foreach (var item in tierItemsList)
            {
                tierItemsDictionary.Add(item.itemName, item);
            }

            foreach (Transform child in transform)
            {
                IObjectPool obj = (IObjectPool) child.GetComponent(typeof(IObjectPool));
                obj.Initialize();
                poolTierDictionary.Add(child.gameObject.name, obj);
            }
        }

        public void UpdateInspectorValues()
        {
            generatorName = gameObject.name;
        }

        public string generatorName { get; set; }

        public int GetPoolsCount()
        {
            throw new System.NotImplementedException();
        }

        public IObjectPool GetPool(string str)
        {
            string loot = tier.RollForTierLoot(str);
            return poolTierDictionary[loot];
        }

        public void SleepPools()
        {
            throw new System.NotImplementedException();
        }
    }
}