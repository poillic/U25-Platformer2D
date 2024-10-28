using UnityEngine;

public class StateIdle : State
{
    public StateIdle( StateMachineV3 _machine ) : base( _machine ) { }

    public override void OnEnter()
    {
        Debug.Log( "J'entre dans l'état IDLE" );
        machine.srenderer.color = Color.red;
    }

    public override void OnExit()
    {
        Debug.Log( "Je sors dans l'état IDLE" );
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnTriggerEnter()
    {
    }

    public override void OnUpdate()
    {
        if( Time.time >= 5f )
        {
            machine.ChangeState( StateMachineV3.STATE_WALK );
        }
    }
}
