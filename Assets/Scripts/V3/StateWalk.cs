using UnityEngine;

public class StateWalk : State
{
    public StateWalk( StateMachineV3 _machine ) : base( _machine ) { }
    public override void OnEnter()
    {
        //machine.currentSpeed = machine.walkSpeed;
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
        if( machine.rb2d.linearVelocityX == 0f && !machine.IsMoving  )
        {
            machine.ChangeState( StateMachineV3.STATE_IDLE );
        }
        else if( machine.IsJumping )
        {
            machine.ChangeState( StateMachineV3.STATE_JUMP );
        }
        else if ( !machine.isGrounded )
        {
            machine.ChangeState( StateMachineV3.STATE_FALL );
        }
    }
}
