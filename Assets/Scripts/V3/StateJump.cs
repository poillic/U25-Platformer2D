using UnityEngine;

public class StateJump : State
{

    public StateJump( StateMachineV3 _machine ) : base ( _machine ) { }
    public override void OnEnter()
    {
        //machine.rb2d.linearVelocityY = machine.jumpForce;
        machine.rb2d.AddForce( Vector2.up * machine.jumpForce, ForceMode2D.Impulse );
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
        if( machine.rb2d.linearVelocityY < 0f )
        {
            machine.ChangeState( StateMachineV3.STATE_FALL );
        }
    }
}
