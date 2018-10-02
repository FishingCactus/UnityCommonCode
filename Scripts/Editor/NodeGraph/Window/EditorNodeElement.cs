using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class EditorNodeElement : EditorElementBase
{
    // -- PUBLIC

    public NodeElement NodeToDisplay;

    public Vector3 Position
    {
        get
        {
            return new Vector3( ColliderRectangle.center.x, ColliderRectangle.center.y );
        }
    }

    public EditorNodeElement(
        NodeElement node_to_display,
        Action<EditorNodeElement> on_node_deleted,
        Action<EditorNodeElement> on_create_transition
        )
    {
        NodeToDisplay = node_to_display;
        OnNodeDeleteAction = on_node_deleted;
        OnCreateTransitionAction = on_create_transition;

        if( StateStyleTable[0] == null )
        {
            CreateStyles();
        }

        CurrentState = EditorElementState.Normal;

        ColliderRectangle = new Rect( NodeToDisplay.Position.x - DisplayHalfWidth, NodeToDisplay.Position.y - DisplayHalfHeight, DisplayWidth, DisplayHeight );
    }

    public Vector3 GetExitPosition()
    {
        return new Vector3( ColliderRectangle.center.x + DisplayHalfWidth, ColliderRectangle.center.y );
    }

    public Vector3 GetExitPositionFromDirection(
        MoveDirection direction
        )
    {
        switch( direction )
        {
        case MoveDirection.Right:
            {
                return new Vector3( ColliderRectangle.center.x + DisplayHalfWidth, ColliderRectangle.center.y );
            }

        case MoveDirection.Left:
            {
                return new Vector3( ColliderRectangle.center.x - DisplayHalfWidth, ColliderRectangle.center.y );
            }

        case MoveDirection.Up:
            {
                return new Vector3( ColliderRectangle.center.x, ColliderRectangle.center.y + DisplayHalfHeight);
            }

        case MoveDirection.Down:
        default:
            {
                return new Vector3( ColliderRectangle.center.x, ColliderRectangle.center.y - DisplayHalfHeight );
            }
        }
    }

    public Vector3 GetEnterPosition()
    {
        return new Vector3( ColliderRectangle.center.x - DisplayHalfWidth, ColliderRectangle.center.y );
    }

    public Vector3 GetEnterPositionFromDirection(
        MoveDirection direction
        )
    {
        switch( direction )
        {
        case MoveDirection.Left:
            {
                return new Vector3( ColliderRectangle.center.x + DisplayHalfWidth, ColliderRectangle.center.y );
            }

        case MoveDirection.Right:
            {
                return new Vector3( ColliderRectangle.center.x - DisplayHalfWidth, ColliderRectangle.center.y );
            }

        case MoveDirection.Down:
            {
                return new Vector3( ColliderRectangle.center.x, ColliderRectangle.center.y + DisplayHalfHeight );
            }

        case MoveDirection.Up:
        default:
            {
                return new Vector3( ColliderRectangle.center.x, ColliderRectangle.center.y - DisplayHalfHeight );
            }
        }
    }

    public bool IsCompleted
    {
        get { return NodeToDisplay.ItIsCompleted; }
    }

    public override void Draw(
        Vector2 window_drag_offset,
        bool it_allows_only_left_right_positions
        )
    {
        Rect display_rectangle = ColliderRectangle;
        display_rectangle.center -= window_drag_offset;

        GUI.Box( display_rectangle, NodeToDisplay.Name, StateStyleTable[ (int)CurrentState ] );

        // TODO : Draw enter & exit markers depending of 'it_allows_only_left_right_positions'.
    }

    // -- PROTECTED

    protected override void ProcessContextMenu(
        Vector2 mousePosition
        )
    {
        GenericMenu genericMenu = new GenericMenu();

        genericMenu.AddItem( new GUIContent( "Create transition" ), false, () => OnCreateTransitionNode( mousePosition ) );
        genericMenu.AddItem( new GUIContent( "Delete node" ), false, () => OnDeleteNode( mousePosition ) );
        genericMenu.ShowAsContext();
    }

    protected override void OnDrag(
        Vector2 drag_offset
        )
    {
        Vector2 new_position = NodeToDisplay.Position + drag_offset;

        NodeToDisplay.Position = new_position;

        ColliderRectangle = new Rect( NodeToDisplay.Position.x - DisplayHalfWidth, NodeToDisplay.Position.y - DisplayHalfHeight, DisplayWidth, DisplayHeight );
    }

    // -- PRIVATE

    private event Action<EditorNodeElement> OnNodeDeleteAction;
    private event Action<EditorNodeElement> OnCreateTransitionAction;

    private GUIStyle[] StateStyleTable = new GUIStyle[(int)EditorElementState.Count];
    private static readonly float DisplayWidth = 100.0f;
    private static readonly float DisplayHalfWidth = 50.0f;
    private static readonly float DisplayHeight = 25.0f;
    private static readonly float DisplayHalfHeight = 12.5f;

    private void CreateStyles()
    {
        StateStyleTable[(int)EditorElementState.Normal] = new GUIStyle();
        StateStyleTable[(int)EditorElementState.Normal].normal.background = EditorGUIUtility.Load( "normal_node.png" ) as Texture2D;
        StateStyleTable[(int)EditorElementState.Normal].border = new RectOffset( 12, 12, 12, 12 );
        StateStyleTable[(int)EditorElementState.Normal].alignment = TextAnchor.MiddleCenter;

        StateStyleTable[(int)EditorElementState.Over] = new GUIStyle();
        StateStyleTable[(int)EditorElementState.Over].normal.background = EditorGUIUtility.Load( "over_node.png" ) as Texture2D;
        StateStyleTable[(int)EditorElementState.Over].border = new RectOffset( 12, 12, 12, 12 );
        StateStyleTable[(int)EditorElementState.Over].alignment = TextAnchor.MiddleCenter;

        StateStyleTable[(int)EditorElementState.Selected] = new GUIStyle();
        StateStyleTable[(int)EditorElementState.Selected].normal.background = EditorGUIUtility.Load( "selected_node.png" ) as Texture2D;
        StateStyleTable[(int)EditorElementState.Selected].border = new RectOffset( 12, 12, 12, 12 );
        StateStyleTable[(int)EditorElementState.Selected].alignment = TextAnchor.MiddleCenter;

        StateStyleTable[(int)EditorElementState.Disabled] = new GUIStyle();
        StateStyleTable[(int)EditorElementState.Disabled].normal.background = EditorGUIUtility.Load( "disabled_node.png" ) as Texture2D;
        StateStyleTable[(int)EditorElementState.Disabled].border = new RectOffset( 12, 12, 12, 12 );
        StateStyleTable[(int)EditorElementState.Disabled].alignment = TextAnchor.MiddleCenter;
    }

    private void OnCreateTransitionNode(
        Vector2 mousePosition
        )
    {
        OnCreateTransitionAction( this );
    }

    private void OnDeleteNode(
        Vector2 mousePosition
        )
    {
        OnNodeDeleteAction( this );
    }
}
