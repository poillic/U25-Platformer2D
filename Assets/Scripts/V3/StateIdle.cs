using UnityEngine;

public class StateIdle : State
{
    public StateIdle( StateMachineV3 _machine ) : base( _machine ) { }

    public override void OnEnter()
    {
        //machine.currentSpeed = 0f;
    }

    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
        machine.HorizontalControl();
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
        else if ( machine.IsJumping )
        {
            machine.ChangeState( StateMachineV3.STATE_JUMP );
        }
        else if( !machine.isGrounded )
        {
            machine.ChangeState( StateMachineV3.STATE_FALL );
        }
    }
}
