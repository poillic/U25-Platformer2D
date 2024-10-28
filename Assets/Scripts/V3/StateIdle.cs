using UnityEngine;

public class StateIdle : State
{
    public StateIdle( StateMachineV3 _machine ) : base( _machine ) { }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnTriggerEnter()
    {
    }

    public override void OnUpdate()
    {
        if( machine.IsMoving )
        {
            machine.ChangeState( StateMachineV3.STATE_WALK );
        }
    }
}
