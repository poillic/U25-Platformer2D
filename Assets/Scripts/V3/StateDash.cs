using UnityEngine;

public class StateDash : State
{
    public StateDash( StateMachineV3 _machine ) : base( _machine ) { }
    private float speedBeforeDash = 0f;
    private float gravityScaleBeforeDash = 0f;
    private float chrono = 0f;
    public override void OnEnter()
    {
        speedBeforeDash = machine.rb2d.linearVelocityX;
        gravityScaleBeforeDash = machine.rb2d.gravityScale;
        machine.rb2d.gravityScale = 0f;
        chrono = 0f;
        machine.dashPressed = false;
        machine.dashAvailable = false;
        machine.currentMaxSpeed = machine.dashSpeed;
        machine.Dash();
    }

    public override void OnExit()
    {
        //machine.currentMaxSpeed = speedBeforeDash;
        machine.rb2d.gravityScale = gravityScaleBeforeDash;
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnTriggerEnter()
    {
    }

    public override void OnUpdate()
    {
        chrono += Time.deltaTime;
        if ( chrono >= machine.dashDuration )
        {
            machine.ChangeState( StateMachineV3.STATE_FALL );
        }
        else if ( machine.jumpBuffer && machine.CanJump )
        {
            machine.ChangeState( StateMachineV3.STATE_JUMP );
        }
    }
}
