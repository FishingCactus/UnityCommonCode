using UnityEditor;
using UnityEngine;

namespace FishingCactus
{
    public class ApplySelectedPrefabs : EditorWindow
    {
        public delegate void ApplyOrRevert( GameObject go_current_gameobject, Object object_prefab_parent );

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
                bool bCanApply;

                foreach ( GameObject go in gameobjects )
                {
                    //Is the selected gameobject a prefab?
                    if ( PrefabUtility.IsPartOfPrefabInstance( go ) || PrefabUtility.IsPrefabAssetMissing( go ) )
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
                            apply_or_revert( goCur, GetCorrespondingObjectOrPrefabParent( goCur ) );
                        }
                    }
                }
                Debug.Log( iCount + " prefab" + ( iCount > 1 ? "s" : "" ) + " updated" );
            }
        }

        static void ApplyToSelectedPrefabs( GameObject _goCurrentGo, Object _ObjPrefabParent )
        {
            PrefabUtility.SaveAsPrefabAssetAndConnect(_goCurrentGo, AssetDatabase.GetAssetPath(_ObjPrefabParent), InteractionMode.UserAction);
        }

        static void RevertToSelectedPrefabs( GameObject _goCurrentGo, Object _ObjPrefabParent )
        {
            PrefabUtility.RevertPrefabInstance( _goCurrentGo , InteractionMode.UserAction );
        }
    }
}