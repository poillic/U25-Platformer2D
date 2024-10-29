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
        machine.rb2d.gravityScale = 1f;
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
    }
}
