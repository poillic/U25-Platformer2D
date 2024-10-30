using UnityEngine;

public class StateJump : State
{

    public StateJump( StateMachineV3 _machine ) : base ( _machine ) { }
    public override void OnEnter()
    {
        float jumpSpeed = Mathf.Sqrt( -2f * Physics2D.gravity.y * machine.rb2d.gravityScale * machine.jumpHeight );
        machine.rb2d.linearVelocityY = jumpSpeed;
        machine.jumpBuffer = false;
        machine.IsJumping = true;
        //machine.rb2d.AddForce( Vector2.up * jumpSpeed, ForceMode2D.Impulse );
        //machine.IsJumping = false;
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
        if ( machine.CanDash )
        {
            machine.ChangeState( StateMachineV3.STATE_DASH );
        }
        else if( machine.rb2d.linearVelocityY < 0f || !machine.jumpBtnPressed )
        {
            machine.ChangeState( StateMachineV3.STATE_FALL );
        }
    }
}
