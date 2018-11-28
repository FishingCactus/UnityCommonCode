using UnityEngine;

[System.Serializable]
public class AnimatorLayerController
{
    // -- PUBLIC

    public void Setup(
        Animator linked_animator
        )
    {
        LinkedAnimator = linked_animator;

        Debug.Assert( !string.IsNullOrEmpty( LayerName), "AnimatorLayerController  : no layer name." );

        LayerIndex = LinkedAnimator.GetLayerIndex( LayerName );

        LayerWeight = 0.0f;
        LinkedAnimator.SetLayerWeight( LayerIndex, LayerWeight );

        InternalState = State.Disabled;
    }

    public void Enable()
    {
        if( InternalState == State.Disabled )
        {
            InternalState = State.Enabling;
        }
    }

    public void Disable()
    {
        if( InternalState == State.Enabled )
        {
            InternalState = State.Disabling;
        }
    }

    public void Update(
        float delta_time
        )
    {
        switch( InternalState )
        {
            case State.Enabling:
            {
                LayerWeight += EnablingSpeed * Time.deltaTime;

                if( LayerWeight >= 1.0f )
                {
                    LayerWeight = 1.0f;
                    InternalState = State.Enabled;
                }

                LinkedAnimator.SetLayerWeight( LayerIndex, LayerWeight );
            }
            break;

            case State.Disabling:
            {
                LayerWeight -= DisablingSpeed * Time.deltaTime;

                if( LayerWeight <= 0.0f )
                {
                    LayerWeight = 0.0f;
                    InternalState = State.Disabled;
                }

                LinkedAnimator.SetLayerWeight( LayerIndex, LayerWeight );
            }
            break;
        }
    }

    // -- PRIVATE

    private enum State
    {
        Disabled = 0,
        Enabling,
        Enabled,
        Disabling
    }

    [SerializeField]
    private string LayerName;
    [SerializeField]
    private float EnablingSpeed = 1.0f;
    [SerializeField]
    private float DisablingSpeed = 1.0f;

    private Animator LinkedAnimator;
    private State InternalState;
    private int LayerIndex;
    private float LayerWeight;
    private Coroutine UpdateRoutine;
}
