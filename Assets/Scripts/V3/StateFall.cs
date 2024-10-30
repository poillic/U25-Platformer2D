using UnityEngine;

public class StateFall : State
{
    public StateFall( StateMachineV3 _machine ) : base( _machine ) { }

    public override void OnEnter()
    {
        machine.rb2d.gravityScale = machine.fallMultiplier;
    }

    public override void OnExit()
    {
        machine.IsJumping = false;
        machine.dashAvailable = true;
        machine.rb2d.gravityScale = 1f;
    }

    public override void OnFixedUpdate()
    {
        machine.HorizontalControl();
        machine.rb2d.linearVelocityY = Mathf.Max( machine.rb2d.linearVelocityY, machine.maxFallSpeed );
    }

    public override void OnTriggerEnter()
    {
    }

    public override void OnUpdate()
    {
        if ( machine.isGrounded )
        {
            if( machine.IsMoving )
            {
                machine.ChangeState( StateMachineV3.STATE_WALK );
            }
            else
            {
                machine.ChangeState( StateMachineV3.STATE_IDLE );
            }
        }
        else
        {
            if ( machine.CanDash )
            {
                machine.ChangeState( StateMachineV3.STATE_DASH );
            }
            else if ( machine.jumpBuffer && machine.CanJump )
            {
                machine.ChangeState( StateMachineV3.STATE_JUMP );
            }
        }
    }
}
