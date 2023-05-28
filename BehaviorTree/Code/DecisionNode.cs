using System.Collections.Generic;
using UnityEngine;

public abstract class DecisionNode : MonoBehaviour
{
    /// <summary>
    /// The root of the structure.
    /// </summary>
    public DecisionNode root;

    /// <summary>
    /// The children nodes of this node.
    /// </summary>
    public List<DecisionNode> children;

    /// <summary>
    /// The index of the best scored child node.
    /// </summary>
    private int _index;

    /// <summary>
    /// Calculates the score of this node.
    /// </summary>
    /// <returns>A score resulting from this node's decision.</returns>
    protected abstract int CalculateScore();

    /// <summary>
    /// Enacts the behavior of the node.
    /// </summary>
    /// <returns>True if able to act, false if failed to enact.</returns>
    public abstract bool Enact();

    /// <summary>
    /// Calculates the score of all children nodes and returns the index of the best score.
    /// </summary>
    /// <returns>Index of best scored node, otherwise -1</returns>
    protected abstract int IndexOfBestChildScore();


    /// <summary>
    /// Calculates the score of all children nodes and returns the index of the best score.
    /// </summary>
    /// <returns>The child node with the best score, otherwise the root node.</returns>
    /// <remarks>
    /// If the index returned from <see cref="IndexOfBestChildScore"/>
    /// returns -1, the root node of the behavior tree will be returned.
    /// </remarks>
    public virtual DecisionNode AdvanceNode()
    {
        _index = IndexOfBestChildScore();
        return _index == -1 ? root : children[_index];
    }

    /// <summary>
    /// Adds a child node to this node.
    /// </summary>
    /// <param name="child">The child node to be added.</param>
    public virtual void AddChild(DecisionNode child) =>
        children.Add(child);
}