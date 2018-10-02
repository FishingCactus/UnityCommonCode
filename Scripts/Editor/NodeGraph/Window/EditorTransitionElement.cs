using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.EventSystems;

[System.Serializable]
public class EditorTransitionElement : EditorElementBase
{
    // -- PUBLIC

    public TransitionElement TransitionToDisplay;

    public static Vector3 GetTangeant(
        MoveDirection direction
        )
    {
        switch( direction )
        {
        case MoveDirection.Right:
            {
                return Vector3.right * 50.0f;
            }

        case MoveDirection.Left:
            {
                return Vector3.left * 50.0f;
            }

        case MoveDirection.Up:
            {
                return Vector3.up * 50.0f;
            }

        case MoveDirection.Down:
        default:
            {
                return Vector3.down * 50.0f;
            }
        }
    }

    public static MoveDirection GetTransitionDirection(
        Vector2 first_position,
        Vector2 second_position
        )
    {
        float x_delta = second_position.x - first_position.x;
        float y_delta = second_position.y - first_position.y;

        if( Mathf.Abs( y_delta ) > Mathf.Abs( x_delta ) )
        {
            return y_delta > 0.0f ? MoveDirection.Up : MoveDirection.Down;
        }
        else
        {
            return x_delta > 0.0f ? MoveDirection.Right : MoveDirection.Left;
        }
    }

    public static void DrawTransitionToCreate(
        EditorNodeElement start_node,
        Vector2 mouse_position,
        Vector2 window_drag_offset,
        bool it_allows_only_left_right_positions
        )
    {
        Vector3 start_point;
        Vector3 end_point;
        MoveDirection transition_direction;

        Vector3 drag_offset = new Vector3( window_drag_offset.x, window_drag_offset.y );

        if( it_allows_only_left_right_positions )
        {
            transition_direction = MoveDirection.Right;
            start_point = start_node.GetExitPosition() - drag_offset;
        }
        else
        {
            transition_direction = GetTransitionDirection( start_node.Position, mouse_position );
            start_point = start_node.GetExitPositionFromDirection( transition_direction ) - drag_offset;
        }

        end_point = new Vector3( mouse_position.x, mouse_position.y, 0.0f );

        Handles.DrawBezier(
            start_point - drag_offset,
            end_point ,
            start_point - drag_offset + GetTangeant( transition_direction ),
            end_point - GetTangeant( transition_direction ),
            Color.blue,
            null,
            2f
            );
    }

    public EditorTransitionElement(
        TransitionElement transition_to_display,
        Action<EditorTransitionElement> on_delete_transition,
        EditorNodeElement first_node,
        EditorNodeElement second_node
        )
    {
        TransitionToDisplay = transition_to_display;
        OnDeleteTransitionAction = on_delete_transition;

        FirstNode = first_node;
        SecondNode = second_node;

        ColliderRectangle = new Rect(
            ( first_node.Position + second_node.Position ) * 0.5f - RectangleHalfSize,
            RectangleSize
            );
    }

    public override void Draw(
        Vector2 window_drag_offset,
        bool it_allows_only_left_right_positions
        )
    {
        Vector3 start_point;
        Vector3 end_point;
        MoveDirection transition_direction;

        Vector3 drag_offset = new Vector3( window_drag_offset.x, window_drag_offset.y );

        if( it_allows_only_left_right_positions )
        {
            transition_direction = MoveDirection.Right;
            start_point = FirstNode.GetExitPosition() - drag_offset;
            end_point = SecondNode.GetEnterPosition() - drag_offset;
        }
        else
        {
            transition_direction = GetTransitionDirection( FirstNode.Position, SecondNode.Position );
            start_point = FirstNode.GetExitPositionFromDirection( transition_direction ) - drag_offset;
            end_point = SecondNode.GetEnterPositionFromDirection( transition_direction ) - drag_offset;
        }

        Handles.DrawBezier(
            start_point,
            end_point,
            start_point + GetTangeant( transition_direction ),
            end_point - GetTangeant( transition_direction ),
            GetCurrentColor(),
            null,
            2f
            );

        Rect display_rectangle = ColliderRectangle;
        display_rectangle.center -= window_drag_offset;

        GUI.backgroundColor = GetCurrentColor();
        GUI.Box( display_rectangle, "" );

        ColliderRectangle.center = ( FirstNode.Position + SecondNode.Position ) * 0.5f;
    }

    // -- PRIVATE

    private readonly Vector3 RectangleSize = new Vector3( 10.0f, 10.0f, 0.0f );
    private readonly Vector3 RectangleHalfSize = new Vector3( 5.0f, 5.0f, 0.0f );

    private event Action<EditorTransitionElement> OnDeleteTransitionAction;

    private EditorNodeElement FirstNode;
    private EditorNodeElement SecondNode;

    private Color GetCurrentColor()
    {
        switch( CurrentState )
        {
            case EditorElementState.Normal:
            {
                return FirstNode.IsCompleted ? Color.green : Color.red;
            }

            case EditorElementState.Over:
            {
                return Color.yellow;
            }

            case EditorElementState.Selected:
            {
                return Color.blue;
            }
        }

        return Color.grey;
    }

    protected override void ProcessContextMenu(
        Vector2 mousePosition
        )
    {
        GenericMenu genericMenu = new GenericMenu();

        genericMenu.AddItem( new GUIContent( "Delete transition" ), false, () => OnDeleteTransition( mousePosition ) );
        genericMenu.ShowAsContext();
    }

    private void OnDeleteTransition(
        Vector2 mousePosition
        )
    {
        OnDeleteTransitionAction( this );
    }
}
