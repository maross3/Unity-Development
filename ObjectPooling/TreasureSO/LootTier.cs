using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Loot/LootTier")]
public class LootTier : ScriptableObject
{
    [InlineEditor(InlineEditorModes.FullEditor)]
    public List<TierItem> itemsInTier;
    
    public string RollForTierLoot(string tier)
    {

        List<TierItem> thisLootTierTreasure = new List<TierItem>();
        int modifierValue = 0;
        foreach (var treasure in itemsInTier) {
            modifierValue += treasure.dropModifier;
            thisLootTierTreasure.Add(treasure);
            
        }

        if (thisLootTierTreasure.Count == 0) {
            Debug.Log($"Loot Tier {tier} was found empty!");
            return null;
        }

        float randomModify = Random.Range(0, modifierValue);
            
        foreach (var treasure in thisLootTierTreasure) {
            if (randomModify < treasure.dropModifier) {
                return treasure.itemName;
            } 
            randomModify -= treasure.dropModifier;
        }
            
        return null;
    }
    
    // Editor tool
    [PropertySpace(SpaceBefore = 0, SpaceAfter = 10)]
    [InfoBox("Use the debug roll to simulate a drop the specified number of times")]
    [HorizontalGroup("testingGroup")]
    [Button("Debug Roll")]
    public void RollForTierLootDebug()
    {
        //define dictionary to display back to usr
        Dictionary<string, int> debugTreasureCount = new Dictionary<string, int>();
        foreach (var treasure in itemsInTier) {
            if (debugTreasureCount.ContainsKey(treasure.itemName)) {
                Debug.Log("Duplicate name found! Did you name each treasure? Accidental duplicates?");
                return;
            }
            debugTreasureCount.Add(treasure.itemName, 0);
            
        }
        
        // simulate rolls debug count times
        for (int i = 0; i < debugRollsToCount; i++) {
            int modifierValue = 0;
            foreach (var treasure in itemsInTier) {
                modifierValue += treasure.dropModifier;
            }
            float randomModify = Random.Range(0, modifierValue);
            foreach (var treasure in itemsInTier) {
                if (randomModify < treasure.dropModifier) {
                    debugTreasureCount[treasure.itemName] += 1;
                    break;
                }
                randomModify -= treasure.dropModifier;
            }
        }
        
        //display results
        foreach (var test in debugTreasureCount) {
            Debug.Log($"Treausure: {test.Key}     Count: {test.Value}");
        }
        
    }
    

    [HorizontalGroup("testingGroup")]
    [SerializeField] public int debugRollsToCount;
    
    
    
}
