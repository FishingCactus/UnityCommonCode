using UnityEditor;
using UnityEngine;

namespace FishingCactus
{
    public class ApplySelectedPrefabs : EditorWindow
    {
        public delegate void ApplyOrRevert( GameObject go_current_gameobject, Object object_prefab_parent, ReplacePrefabOptions replace_options );

        [MenuItem( "FishingCactus/Tools/Apply all selected prefabs %#a" )]
        static void ApplyPrefabs()
        {
            SearchPrefabConnections( ApplyToSelectedPrefabs );
        }

        [MenuItem( "FishingCactus/Tools/Revert all selected prefabs %#r" )]
        static void ResetPrefabs()
        {
            SearchPrefabConnections( RevertToSelectedPrefabs );
        }

        // -- PRIVATE

        private static Object GetCorrespondingObjectOrPrefabParent(
            GameObject object_to_use
            )
        {
            #if UNITY_2018_2_OR_NEWER
                return PrefabUtility.GetCorrespondingObjectFromSource( object_to_use );
            #else
                return PrefabUtility.GetPrefabParent( object_to_use );
            #endif
        }

        //Look for connections
        static void SearchPrefabConnections( ApplyOrRevert apply_or_revert )
        {
            GameObject[] gameobjects = Selection.gameObjects;

            if ( gameobjects.Length > 0 )
            {
                GameObject goPrefabRoot;
                GameObject goCur;
                bool bTopHierarchyFound;
                int iCount = 0;
                PrefabType prefabType;
                bool bCanApply;

                foreach ( GameObject go in gameobjects )
                {
                    prefabType = PrefabUtility.GetPrefabType( go );
                    //Is the selected gameobject a prefab?
                    if ( prefabType == PrefabType.PrefabInstance || prefabType == PrefabType.DisconnectedPrefabInstance )
                    {
                        //Prefab Root;
                        goPrefabRoot = ( ( GameObject )GetCorrespondingObjectOrPrefabParent( go ) ).transform.root.gameObject;
                        goCur = go;
                        bTopHierarchyFound = false;
                        bCanApply = true;
                        //We go up in the hierarchy to apply the root of the go to the prefab
                        while ( goCur.transform.parent != null && !bTopHierarchyFound )
                        {
                            //Are we still in the same prefab?
                            if ( goPrefabRoot == ( ( GameObject )GetCorrespondingObjectOrPrefabParent( goCur.transform.parent.gameObject ) ).transform.root.gameObject )
                            {
                                goCur = goCur.transform.parent.gameObject;
                            }
                            else
                            {
                                //The gameobject parent is another prefab, we stop here
                                bTopHierarchyFound = true;
                                if ( goPrefabRoot != ( ( GameObject )GetCorrespondingObjectOrPrefabParent( goCur ) ) )
                                {
                                    //Gameobject is part of another prefab
                                    bCanApply = false;
                                }
                            }
                        }

                        if ( apply_or_revert != null && bCanApply )
                        {
                            iCount++;
                            apply_or_revert( goCur, GetCorrespondingObjectOrPrefabParent( goCur ), ReplacePrefabOptions.ConnectToPrefab );
                        }
                    }
                }
                Debug.Log( iCount + " prefab" + ( iCount > 1 ? "s" : "" ) + " updated" );
            }
        }

        static void ApplyToSelectedPrefabs( GameObject _goCurrentGo, Object _ObjPrefabParent, ReplacePrefabOptions _eReplaceOptions )
        {
            PrefabUtility.ReplacePrefab( _goCurrentGo, _ObjPrefabParent, _eReplaceOptions );
        }

        static void RevertToSelectedPrefabs( GameObject _goCurrentGo, Object _ObjPrefabParent, ReplacePrefabOptions _eReplaceOptions )
        {
            PrefabUtility.ReconnectToLastPrefab( _goCurrentGo );
            PrefabUtility.RevertPrefabInstance( _goCurrentGo );
        }
    }
}