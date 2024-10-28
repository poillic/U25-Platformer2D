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
    }

    public override void OnTriggerEnter()
    {
    }

    public override void OnUpdate()
    {
        if ( machine.rb2d.linearVelocityY == 0f )
        {
            machine.ChangeState( StateMachineV3.STATE_IDLE );
        }
    }
}
