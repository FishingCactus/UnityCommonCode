using UnityEngine;

[System.Serializable]
public abstract class EditorElementBase
{
    // -- PUBLIC

    public EditorElementState CurrentState
    {
        get;
        protected set;
    }

    public bool IsMouseOver(
        Vector2 zoomed_mouse_position
        )
    {
        return ColliderRectangle.Contains( zoomed_mouse_position );
    }

    public abstract void Draw(
        Vector2 window_drag_offset,
        bool it_allows_only_left_right_positions
        );

    // -- PROTECTED

    protected Rect ColliderRectangle;
    protected bool ItIsDraggable = true;
    protected bool ItIsDragged = false;

    protected abstract void ProcessContextMenu(
        Vector2 mousePosition
        );

    protected virtual void OnDrag(
        Vector2 drag_offset
        )
    {
    }

    // -- UNITY

    public bool ProcessEvents(
        Event event_to_process,
        Vector2 zoomed_mouse_position,
        float zoom_value
        )
    {
        bool event_has_been_processed = false;

        switch( event_to_process.type )
        {
        case EventType.MouseMove:
            {
                if( IsMouseOver( zoomed_mouse_position ) )
                {
                    if( CurrentState == EditorElementState.Normal )
                    {
                        CurrentState = EditorElementState.Over;
                        event_has_been_processed = true;
                    }
                }
                else if( CurrentState == EditorElementState.Over )
                {
                    CurrentState = EditorElementState.Normal;
                    event_has_been_processed = true;
                }
            }
            break;

        case EventType.MouseDown:
            {
                if( IsMouseOver( zoomed_mouse_position ) )
                {
                    if( event_to_process.button == 0 )
                    {
                        ItIsDragged = ItIsDraggable;

                        CurrentState = EditorElementState.Selected;
                        event_has_been_processed = true;
                    }
                    else if( event_to_process.button == 1 )
                    {
                        CurrentState = EditorElementState.Selected;
                        event_has_been_processed = true;
                    }
                }
                else if( CurrentState != EditorElementState.Normal )
                {
                    CurrentState = EditorElementState.Normal;
                    event_has_been_processed = true;
                }
            }
            break;

        case EventType.MouseDrag:
            {
                if( event_to_process.button == 0
                    && ItIsDragged
                    )
                {
                    OnDrag( event_to_process.delta / zoom_value );
                    event_has_been_processed = true;
                }
            }
            break;

        case EventType.MouseUp:
            {
                ItIsDragged = false;

                if( IsMouseOver( zoomed_mouse_position ) )
                {
                    if( event_to_process.button == 1 )
                    {
                        CurrentState = EditorElementState.Selected;
                        ProcessContextMenu( zoomed_mouse_position );
                        event_has_been_processed = true;
                    }
                    else if( event_to_process.button == 0 )
                    {
                        event_has_been_processed = true;
                    }
                }
            }
            break;
        }

        return event_has_been_processed;
    }
}
