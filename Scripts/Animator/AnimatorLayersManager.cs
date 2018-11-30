﻿using System.Collections;
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
                if( it_must_enable_layer 
                    && layer_controller.CanBeEnabled
                    )
                {
                    layer_controller.Enable();
                }
                else if( !it_must_enable_layer
                    && layer_controller.CanBeDisabled
                    )
                {
                    layer_controller.Disable();
                }

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

    // -- UNITY

    private void Awake()
    {
        foreach( AnimatorLayerController layer_controller in LayerControllerTable )
        {
            layer_controller.Setup( LinkedAnimator );
        }
    }

    private void LateUpdate()
    {
        foreach( AnimatorLayerController layer_controller in LayerControllerTable )
        {
            layer_controller.UpdateWeight( Time.deltaTime );
        }
    }
}
