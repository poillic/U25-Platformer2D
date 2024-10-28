using UnityEngine;

public abstract class State
{
    protected StateMachineV3 machine;

    public State( StateMachineV3 _machine )
    {
        machine = _machine;
    }

    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnTriggerEnter();
    public abstract void OnEnter();
    public abstract void OnExit();
}
