using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "NodeGraph_", menuName = "NodeGraph", order = 1 )]
public class NodeGraph : ScriptableObject
{
    // -- PUBLIC

    public List<NodeElement> NodeTable;
    public List<TransitionElement> TransitionTable;
    public NodeElement SelectedNode;
    public TransitionElement SelectedTransition;
    public bool ItIsLinear = true;

    [System.NonSerialized]
    public int SelectedElementIndex;

    public void AddNode(
        string name,
        int index,
        Vector2 position
        )
    {
        NodeElement new_transition = new NodeElement();
        new_transition.Setup( name, index, position );

        NodeTable.Add( new_transition );
    }

    public void RemoveNode(
        NodeElement node_to_delete
        )
    {
        NodeTable.Remove( node_to_delete );

        for( int transition_index = TransitionTable.Count - 1; transition_index >= 0; transition_index-- )
        {
            if( TransitionTable[transition_index].ContainsNode( node_to_delete ) )
            {
                TransitionTable.RemoveAt( transition_index );
            }
        }
    }

    public void AddTransition(
        int index,
        NodeElement first_node,
        NodeElement second_node
        )
    {
        TransitionElement new_transition = new TransitionElement();
        new_transition.Setup( index, first_node, second_node );

        TransitionTable.Add( new_transition );
    }

    public void RemoveTransition(
        TransitionElement transition_to_delete
        )
    {
        TransitionTable.Remove( transition_to_delete );
    }

    public void Select(
        NodeElement selected_node,
        TransitionElement selected_transition
        )
    {
        SelectedNode = selected_node;
        SelectedTransition = selected_transition;

        if( selected_node != null )
        {
            SelectedElementIndex = SelectedNode.Index;
        }
        else if( selected_transition != null )
        {
            SelectedElementIndex = SelectedTransition.Index;
        }
        else
        {
            SelectedElementIndex = 0;
        }
    }

    public virtual bool Update()
    {
        return false;
    }
}
