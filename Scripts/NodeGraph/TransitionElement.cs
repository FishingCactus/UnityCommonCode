using UnityEngine;

[System.Serializable]
public class TransitionElement : ElementBase
{
    // -- PUBLIC

    [HideInInspector]
    public NodeElement FirstNode;
    [HideInInspector]
    public NodeElement SecondNode;

    public TransitionElement() : base()
    {
    }

    public void Setup(
        int index,
        NodeElement first_node,
        NodeElement second_node
        )
    {
        Name = $"Transition_{first_node.Name}-{second_node.Name}";

        Index = index;

        FirstNode = first_node;
        SecondNode = second_node;
    }

    public bool ContainsNode(
        NodeElement node_to_check
        )
    {
        return FirstNode.Index == node_to_check.Index || SecondNode.Index == node_to_check.Index;
    }
}
