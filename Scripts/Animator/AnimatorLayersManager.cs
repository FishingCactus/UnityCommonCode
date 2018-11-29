using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorLayersManager : MonoBehaviour
{
    // -- PUBLIC

    public int GetLayerId(
        string layer_name
        )
    {
        foreach( AnimatorLayerController layer_controller in LayerControllerTable )
        {
            if( layer_controller.LayerName.Equals( layer_name ) )
            {
                return layer_controller.LayerIndex;
            }
        }

        Debug.LogError( $"{LinkedAnimator.name} has no layer called {layer_name}." );

        return -1;
    }

    public void SetLayerEnabled(
        int layer_index,
        bool it_must_enable_layer
        )
    {
        foreach( AnimatorLayerController layer_controller in LayerControllerTable )
        {
            if( layer_controller.LayerIndex == layer_index )
            {
                if( layer_controller.UpdateRoutine != null )
                {
                    StopCoroutine( layer_controller.UpdateRoutine );
                }
                    
                layer_controller.UpdateRoutine = StartCoroutine( LayerUpdateRoutine( layer_controller, it_must_enable_layer ) );

                return;
            }
        }

        Debug.LogError( $"{LinkedAnimator.name} has no layer at index {layer_index}." );
    }

    // -- PRIVATE

    [SerializeField]
    private Animator LinkedAnimator;
    [SerializeField]
    private List<AnimatorLayerController> LayerControllerTable = new List<AnimatorLayerController>();

    private IEnumerator LayerUpdateRoutine(
        AnimatorLayerController layer_to_update,
        bool it_must_enable_layer
        )
    {
        if( it_must_enable_layer )
        {
            if( !layer_to_update.CanBeEnabled )
            {
                yield break;
            }

            layer_to_update.Enable();
        }
        else
        {
            if( !layer_to_update.CanBeDisabled )
            {
                yield break;
            }

            layer_to_update.Disable();
        }

        while( !layer_to_update.UpdateWeight() )
        {
            yield return null;
        }        
    }

    // -- UNITY

    private void Awake()
    {
        foreach( AnimatorLayerController layer_controller in LayerControllerTable )
        {
            layer_controller.Setup( LinkedAnimator );
        }
    }
}
