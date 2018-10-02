using UnityEngine;
using UnityEditor;
using FishingCactus;
using System.Collections.Generic;

public class ZoomedRectangle
{
    private static float WindowTabHeight = 21.0f;
    private static Matrix4x4 PreviousGuiMatrix;

    public static Rect Begin(
        float zoomScale,
        Rect initial_rectangle
        )
    {
        GUI.EndGroup();

        Rect clippedArea = initial_rectangle.Scale( 1.0f / zoomScale, initial_rectangle.TopLeft() );
        clippedArea.y += WindowTabHeight;

        GUI.BeginGroup( clippedArea );

        PreviousGuiMatrix = GUI.matrix;

        Matrix4x4 translation = Matrix4x4.TRS( clippedArea.TopLeft(), Quaternion.identity, Vector3.one );
        Matrix4x4 scale = Matrix4x4.Scale( new Vector3( zoomScale, zoomScale, 1.0f ) );

        GUI.matrix = translation * scale * translation.inverse * GUI.matrix;

        return clippedArea;
    }

    public static void End()
    {
        GUI.matrix = PreviousGuiMatrix;
        GUI.EndGroup();

        GUI.BeginGroup( new Rect( 0.0f, WindowTabHeight, Screen.width, Screen.height ) );
    }
}

public class NodeGraphWindow : EditorWindow
{
    // -- PRIVATE

    private Rect WindowArea = new Rect( 0.0f, 0.0f, 600.0f, 300.0f );
    private Rect ClippedArea = new Rect();
    private float ZoomValue = 1.0f;
    private Vector2 DragOffset = Vector2.zero;
    private NodeGraph EditedNodeGraph;
    private List<EditorNodeElement> NodeTable = new List<EditorNodeElement>();
    private List<EditorTransitionElement> TransitionTable = new List<EditorTransitionElement>();
    private bool ItMustCreateTransition = false;
    private EditorNodeElement TransitionStartNode = null;
    private int LastNodeIndex;

    private const float MinimalZoom = 0.1f;
    private const float MaximumZoom = 10.0f;

    private readonly Color BackgroundColor = new Color32( 106, 106, 106, 255 );

    private void DrawGrid(
        float gridSpacing,
        float gridOpacity,
        Color gridColor
        )
    {
        int widthDivs = Mathf.CeilToInt( ClippedArea.width / gridSpacing );
        int heightDivs = Mathf.CeilToInt( ClippedArea.height / gridSpacing );

        Handles.BeginGUI();
        Handles.color = new Color( gridColor.r, gridColor.g, gridColor.b, gridOpacity );

        Vector3 newOffset = -DragOffset;

        float x_start_grid = DragOffset.x - ( DragOffset.x % gridSpacing);
        float y_start_grid = DragOffset.y - ( DragOffset.y % gridSpacing );

        for( int i = 0; i < widthDivs; i++ )
        {
            Handles.DrawLine(
                new Vector3( x_start_grid + ( gridSpacing * i ), DragOffset.y, 0 ) + newOffset,
                new Vector3( x_start_grid + ( gridSpacing * i ), DragOffset.y + ClippedArea.height, 0f ) + newOffset
                );
        }

        for( int j = 0; j < heightDivs; j++ )
        {
            Handles.DrawLine(
                new Vector3( DragOffset.x, y_start_grid + ( gridSpacing * j ), 0 ) + newOffset,
                new Vector3( DragOffset.x + ClippedArea.width, y_start_grid + ( gridSpacing * j ), 0f ) + newOffset
                );
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private Vector2 ScreenToZoomPosition(
        Vector2 screen_position
        )
    {
        return ( screen_position - WindowArea.TopLeft() ) / ZoomValue + DragOffset;
    }

    private EditorNodeElement GetEditorNode(
        NodeElement node_to_find
        )
    {
        foreach( EditorNodeElement node in NodeTable )
        {
            if( node.NodeToDisplay.Index == node_to_find.Index )
            {
                return node;
            }
        }

        return null;
    }

    private void CreateNodesAndTransitions(
        NodeGraph node_graph
        )
    {
        NodeTable.Clear();
        TransitionTable.Clear();
        LastNodeIndex = 0;

        if( node_graph == null )
        {
            return;
        }

        if( node_graph.NodeTable == null )
        {
            node_graph.NodeTable = new List<NodeElement>();
        }

        if( node_graph.TransitionTable == null )
        {
            node_graph.TransitionTable = new List<TransitionElement>();
        }

        foreach( NodeElement node_to_manage in node_graph.NodeTable )
        {
            NodeTable.Add( new EditorNodeElement( node_to_manage, OnNodeDeleted, OnCreateTransition ) );

            LastNodeIndex = Mathf.Max( LastNodeIndex, node_to_manage.Index );
        }

        foreach( TransitionElement transition_to_manage in node_graph.TransitionTable )
        {
            TransitionTable.Add(
                new EditorTransitionElement(
                    transition_to_manage,
                    OnTransitionDeleted,
                    GetEditorNode( transition_to_manage.FirstNode ),
                    GetEditorNode( transition_to_manage.SecondNode )
                    )
                );

            LastNodeIndex = Mathf.Max( LastNodeIndex, transition_to_manage.Index );
        }
    }

    private void DrawNodes()
    {
        if( NodeTable != null )
        {
            foreach( EditorNodeElement node_to_draw in NodeTable )
            {
                node_to_draw.Draw( DragOffset, EditedNodeGraph.ItIsLinear );
            }
        }
    }

    private void DrawTransitions()
    {
        Color background_color_backup = GUI.backgroundColor;

        if( TransitionTable != null )
        {
            foreach( EditorTransitionElement transition_to_draw in TransitionTable )
            {
                transition_to_draw.Draw( DragOffset, EditedNodeGraph.ItIsLinear );
            }
        }

        GUI.backgroundColor = background_color_backup;
    }

    private void ProcessContextMenu(
        Vector2 mousePosition
        )
    {
        GenericMenu genericMenu = new GenericMenu();

        genericMenu.AddItem( new GUIContent( "Add node" ), false, () => OnCreateNode( mousePosition ) );
        genericMenu.ShowAsContext();
    }

    private void OnCreateNode(
        Vector2 mousePosition
        )
    {
        if( EditedNodeGraph.NodeTable == null )
        {
            EditedNodeGraph.NodeTable = new List<NodeElement>();
        }

        EditedNodeGraph.AddNode( "Node", ++LastNodeIndex, mousePosition );
        CreateNodesAndTransitions( EditedNodeGraph );
    }

    private void OnNodeDeleted(
        EditorNodeElement node_to_delete
        )
    {
        EditedNodeGraph.RemoveNode( node_to_delete.NodeToDisplay );
        CreateNodesAndTransitions( EditedNodeGraph );

        Repaint();
    }

    private void OnCreateTransition(
        EditorNodeElement transition_start_node
        )
    {
        ItMustCreateTransition = true;
        TransitionStartNode = transition_start_node;
    }

    private void OnTransitionDeleted(
        EditorTransitionElement transition_to_delete
        )
    {
        EditedNodeGraph.RemoveTransition( transition_to_delete.TransitionToDisplay );
        CreateNodesAndTransitions( EditedNodeGraph );

        Repaint();
    }

    private void LoadSelectedParameters()
    {
        if( Selection.activeObject
            && Selection.activeObject.GetType() == typeof( NodeGraph )
            )
        {
            if( EditedNodeGraph != null )
            {
                EditorUtility.SetDirty( EditedNodeGraph );
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if( EditedNodeGraph != Selection.activeObject as NodeGraph )
            {
                EditedNodeGraph = Selection.activeObject as NodeGraph;
                EditedNodeGraph.Select( null, null );

                CreateNodesAndTransitions( EditedNodeGraph );

                ZoomValue = 1.0f;
                DragOffset = Vector2.zero;
            }

            Repaint();
        }
    }

    // -- UNITY

    [MenuItem( "Window/Logical Graph Editor" )]
    private static void Init()
    {
        NodeGraphWindow window = GetWindow<NodeGraphWindow>();

        window.titleContent = new GUIContent( "Logical Graph Editor" );
        window.minSize = new Vector2( 600.0f, 300.0f );
        window.wantsMouseMove = true;
        window.Show();

        FocusWindowIfItsOpen( typeof( NodeGraphWindow ) );
    }

    void OnSelectionChange()
    {
        LoadSelectedParameters();
    }

    private void Awake()
    {
        LoadSelectedParameters();
    }

    public void OnGUI()
    {
        Color previous_color = GUI.color;
        GUI.color = BackgroundColor;

        WindowArea.Set( 0.0f, 0.0f, position.width, position.height );

        GUI.DrawTexture( WindowArea, EditorGUIUtility.whiteTexture );
        GUI.color = previous_color;

        ClippedArea = ZoomedRectangle.Begin( ZoomValue, WindowArea );

        DrawGrid( 10, 0.1f, Color.black );
        DrawGrid( 100, 0.2f, Color.black );

        DrawTransitions();
        DrawNodes();

        if( ItMustCreateTransition )
        {
            GUI.changed = true;
            EditorTransitionElement.DrawTransitionToCreate( TransitionStartNode, Event.current.mousePosition, DragOffset, EditedNodeGraph.ItIsLinear );
        }

        ZoomedRectangle.End();

        wantsMouseMove = true;

        if( !ItMustCreateTransition
            || Event.current.type == EventType.MouseMove
            )
        {
            DispatchEventOnNodes();
        }

        HandleEvents();

        if( GUI.changed )
        {
            Repaint();
        }
    }

    private void DispatchEventOnNodes()
    {
        bool event_has_been_processed = false;

        if( TransitionTable != null )
        {
            foreach( EditorTransitionElement transition_to_process in TransitionTable )
            {
                if( transition_to_process.ProcessEvents( Event.current, ScreenToZoomPosition( Event.current.mousePosition ), ZoomValue ) )
                {
                    event_has_been_processed = true;
                }

                if( transition_to_process.CurrentState == EditorElementState.Selected )
                {
                    if( EditedNodeGraph.SelectedTransition != transition_to_process.TransitionToDisplay )
                    {
                        EditedNodeGraph.Select( null, transition_to_process.TransitionToDisplay );
                    }
                }
            }
        }

        if( NodeTable != null )
        {
            foreach( EditorNodeElement node_to_process in NodeTable )
            {
                if( node_to_process.ProcessEvents( Event.current, ScreenToZoomPosition( Event.current.mousePosition ), ZoomValue ) )
                {
                    event_has_been_processed = true;
                }

                if( node_to_process.CurrentState == EditorElementState.Selected )
                {
                    if( EditedNodeGraph.SelectedNode != node_to_process.NodeToDisplay )
                    {
                        EditedNodeGraph.Select( node_to_process.NodeToDisplay, null );
                    }
                }
            }
        }

        if( event_has_been_processed )
        {
            Event.current.Use();
            GUI.changed = true;
        }
    }

    private void HandleEvents()
    {
        switch( Event.current.type )
        {
            case EventType.KeyDown:
            {
                if( Event.current.keyCode == KeyCode.Escape )
                {
                    ItMustCreateTransition = false;

                    Event.current.Use();
                }
            }
            break;

            case EventType.ScrollWheel:
            {
                // Allow adjusting the zoom with the mouse wheel as well. In this case, use the mouse coordinates
                // as the zoom center instead of the top left corner of the zoom area. This is achieved by
                // maintaining an origin that is used as offset when drawing any GUI elements in the zoom area.

                Vector2 zoom_position = ScreenToZoomPosition( Event.current.mousePosition );
                float zoom_delta = -Event.current.delta.y / ( 25.0f / ZoomValue );
                float old_zoom_value = ZoomValue;

                ZoomValue += zoom_delta;
                ZoomValue = Mathf.Clamp( ZoomValue, MinimalZoom, MaximumZoom );

                DragOffset += ( zoom_position - DragOffset ) - ( zoom_position - DragOffset ) * ( old_zoom_value / ZoomValue );

                Event.current.Use();
            }
            break;

            case EventType.MouseDrag:
            {
                if( ( Event.current.button == 0 && Event.current.modifiers == EventModifiers.Alt )
                    || Event.current.button == 2
                    )
                {
                    DragOffset -= Event.current.delta / ZoomValue;

                    Event.current.Use();
                }
            }
            break;

            case EventType.MouseDown:
            {
                if( ItMustCreateTransition )
                {
                    Vector2 zoomed_mouse_position = ScreenToZoomPosition( Event.current.mousePosition );

                    ItMustCreateTransition = false;

                    foreach( EditorNodeElement node_to_check in NodeTable )
                    {
                        if( node_to_check.IsMouseOver( zoomed_mouse_position ) )
                        {
                            EditedNodeGraph.AddTransition( ++LastNodeIndex, TransitionStartNode.NodeToDisplay, node_to_check.NodeToDisplay );
                            CreateNodesAndTransitions( EditedNodeGraph );

                            Repaint();

                            return;
                        }
                    }
                }
            }
            break;

            case EventType.MouseUp:
            {
                if( Event.current.button == 1 )
                {
                    ProcessContextMenu( ScreenToZoomPosition( Event.current.mousePosition ) );
                }
                else if( Event.current.button == 0 )
                {
                    if( EditedNodeGraph != null )
                    {
                        EditedNodeGraph.Select( null, null );
                        Selection.activeObject = EditedNodeGraph;
                    }
                }
            }
            break;
        }
    }
}
