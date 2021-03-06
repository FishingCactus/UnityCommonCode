﻿using UnityEngine;

[System.Serializable]
public class AnimatorLayerController
{
    // -- PUBLIC

    public enum State
    {
        Disabled = 0,
        Enabling,
        Enabled,
        Disabling
    }

    public int LayerIndex { get; private set; }
    public string LayerName { get { return _LayerName; } }
    
    public bool CanBeEnabled { get {  return InternalState == State.Disabled || InternalState == State.Disabling; } }
    public bool CanBeDisabled { get { return InternalState == State.Enabled || InternalState == State.Enabling; } }

    public void Setup(
        Animator linked_animator
        )
    {
        LinkedAnimator = linked_animator;

        LayerIndex = LinkedAnimator.GetLayerIndex( _LayerName );
        Debug.Assert( LayerIndex != -1, $"No layer '{_LayerName}' found in '{linked_animator.name}'." );

        LayerWeight = 0.0f;
        LinkedAnimator.SetLayerWeight( LayerIndex, LayerWeight );

        InternalState = State.Disabled;
    }

    public void Enable()
    {
        if( CanBeEnabled )
        {
            if( EnablingSpeed == 0.0f )
            {
                LayerWeight = 1.0f;
                LinkedAnimator.SetLayerWeight( LayerIndex, LayerWeight );
                InternalState = State.Enabled;
            }
            else
            {
                InternalState = State.Enabling;
            }
        }
    }

    public void Disable()
    {
        if( CanBeDisabled )
        {
            if( DisablingSpeed == 0.0f )
            {
                LayerWeight = 0.0f;
                LinkedAnimator.SetLayerWeight( LayerIndex, LayerWeight );
                InternalState = State.Disabled;
            }
            else
            {
                InternalState = State.Disabling;
            }
        }
    }

    public bool UpdateWeight(
        float delta_time
        )
    {
        if( InternalState == State.Disabled
            || InternalState == State.Enabled
            )
        {
            return true;
        }
        
        switch( InternalState )
        {
            case State.Enabling:
            {
                LayerWeight = Mathf.Min( 1.0f, LayerWeight + EnablingSpeed * delta_time );
                LinkedAnimator.SetLayerWeight( LayerIndex, LayerWeight );

                if( LayerWeight >= 1.0f )
                {
                    InternalState = State.Enabled;

                    return true;
                }
            }
            break;

            case State.Disabling:
            {
                LayerWeight = Mathf.Max( 0.0f, LayerWeight - DisablingSpeed * delta_time );
                LinkedAnimator.SetLayerWeight( LayerIndex, LayerWeight );

                if( LayerWeight <= 0.0f )
                {
                    InternalState = State.Disabled;

                    return true;
                }
            }
            break;
        }

        return false;
    }

    // -- PRIVATE

    #pragma warning disable 0649
    [SerializeField]
    private string _LayerName;
    #pragma warning restore
    [SerializeField]
    private float EnablingSpeed = 1.0f;
    [SerializeField]
    private float DisablingSpeed = 1.0f;

    private Animator LinkedAnimator;
    private State InternalState;
    private float LayerWeight;
}
