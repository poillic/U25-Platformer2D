using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineV3 : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public SpriteRenderer srenderer;


    private Dictionary<string, State> _states = new();
    public State currentState;

    public const string STATE_IDLE = nameof( StateIdle );
    public const string STATE_WALK = nameof( StateWalk );

    public bool IsMoving { 
        get { return _moveDirection != 0f; }
    }
    private float _moveDirection = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _states.Add( STATE_IDLE, new StateIdle( this ) );
        _states.Add( STATE_WALK, new StateWalk( this ) );

        ChangeState( nameof( StateIdle ) );
    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        currentState.OnTriggerEnter();
    }

#if UNITY_EDITOR
    public void OnGUI()
    {
        GUILayout.TextArea( currentState.ToString() );
        GUILayout.TextArea( "Toto" );
    }
#endif

    private void OnDrawGizmos()
    {
        
        if( IsMoving )
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawCube( transform.position, Vector3.one );

        Gizmos.DrawLine( transform.position, transform.right );
    }

    public void ChangeState( string stateName )
    {
        /*if( currentState != null )
        {
            currentState.OnExit();
        }*/

#if UNITY_EDITOR
        Debug.Log( "Exiting : " + nameof( currentState ) );
        Debug.Log( "Entering : " + stateName );
#endif

        currentState?.OnExit();
        currentState = _states[ stateName ];
        currentState.OnEnter();
    }

    public void OnMove( InputAction.CallbackContext context )
    {
        switch ( context.phase )
        {
            case InputActionPhase.Disabled:
                break;
            case InputActionPhase.Waiting:
                break;
            case InputActionPhase.Started:
                break;
            case InputActionPhase.Performed:
                _moveDirection = context.ReadValue<float>();
                break;
            case InputActionPhase.Canceled:
                _moveDirection = 0f;
                break;
            default:
                break;
        }
    }
}
