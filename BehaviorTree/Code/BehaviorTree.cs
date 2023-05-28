using System;
using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    /// <summary>
    /// The root node of the behavior tree.
    /// </summary>
    private DecisionNode _root;

    /// <summary>
    /// The current node of the behavior tree.
    /// </summary>
    private DecisionNode _curNode;

    private void Start()
    {
        _root = gameObject.AddComponent<WanderNode>();
        _root.root = _root;
        _curNode = _root;
    }
    
    /// <summary>
    /// Resets the behavior tree's <seealso cref="_curNode"/> to the root node.
    /// </summary>
    public void Reset() =>
        _curNode = _root;

    /// <summary>
    /// Evaluate the behavior tree.
    /// </summary>
    /// <returns>True if node state did not change, false if node state changed.</returns>
    public bool Evaluate()
    {
        if (_curNode.Enact()) return true;
        _curNode = _curNode.AdvanceNode();
        return false;
    }
}