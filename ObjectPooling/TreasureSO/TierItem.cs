using AssetIcons;
using ObjectPooling;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Loot/TierItem")]
public class TierItem : ScriptableObject
{
    [Required]
    [TitleGroup("Tier Item")]
    public string itemName;

    [SerializeField] [AssetIcon] public Sprite icon;
    public bool stackable;
    
    [Required]
    [HorizontalGroup("Split", 0.25f, LabelWidth = 100)]
    [BoxGroup("Split/Overview")]
    [PreviewField(50, ObjectFieldAlignment.Right)]
    public GameObject tierItemPrefab;

    public TreasureGenerator.ItemValues[] values;

    [BoxGroup("Split/Values")]
    [PropertySpace(SpaceAfter = 4)]
    public int dropModifier;
}
