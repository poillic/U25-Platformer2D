using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineV3 : MonoBehaviour
{
    [Header("Speeds")]
    private float currentSpeed = 0f;
    [HideInInspector] public float currentMaxSpeed = 0f;
    public float acceleration = 52f;
    public float deceleration = 52f;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float dashSpeed = 24f;
    public float dashDuration = 0.2f;
    public float coyoteTimeDuration = 0.2f;
    public float jumpBufferDuration = 0.2f;
    public float maxFallSpeed = -12f;

    [Header( "Air Control" )]
    public float jumpHeight = 2.5f;
    public float fallMultiplier = 1.2f;

    [Header( "Ground Detection" )]
    public bool isGrounded = false;
    public LayerMask groundMask;
    public Transform groundChecker;
    public Vector2 groundCheckerDimension;

    public Animator anim;
    public Rigidbody2D rb2d;
    public SpriteRenderer srenderer;

    private Dictionary<string, State> _states = new();
    public State currentState;

    public const string STATE_IDLE = nameof( StateIdle );
    public const string STATE_WALK = nameof( StateWalk );
    public const string STATE_JUMP = nameof( StateJump );
    public const string STATE_FALL = nameof( StateFall );
    public const string STATE_DASH = nameof( StateDash );

    public bool IsMoving { 
        get { return _moveDirection != 0f; }
    }

    public float MoveDirection
    {
        get { return _moveDirection; }
    }

    public bool CanJump
    {
        get { return ( InCoyoteTime || isGrounded ) && !IsJumping; }
    }

    private float _moveDirection = 0f;
    private float _verticalDirection = 0f;
    public bool IsJumping = false;
    public bool dashPressed = false;
    public bool jumpBtnPressed = false;
    [HideInInspector] public bool dashAvailable = true;
    [HideInInspector] public bool jumpBuffer = false;
    [HideInInspector] public bool InCoyoteTime = false;

    public bool CanDash
    {
        get { return dashPressed && dashAvailable; }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _states.Add( STATE_IDLE, new StateIdle( this ) );
        _states.Add( STATE_WALK, new StateWalk( this ) );
        _states.Add( STATE_JUMP, new StateJump( this ) );
        _states.Add( STATE_FALL, new StateFall( this ) );
        _states.Add( STATE_DASH, new StateDash( this ) );

        ChangeState( nameof( StateIdle ) );
    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate();
        anim.SetFloat( "MoveSpeedX", Mathf.Abs( rb2d.linearVelocityX ) );
        anim.SetFloat( "MoveSpeedY", Mathf.Abs( rb2d.linearVelocityY ) );
        anim.SetBool( "IsJumping", IsJumping );

        if( _moveDirection != 0f )
        {
            transform.localScale = new Vector3( Mathf.Sign( _moveDirection ), 1f, 1f );
        }
    }

    private void FixedUpdate()
    {

        currentState.OnFixedUpdate();
        rb2d.linearVelocityX = currentSpeed;

        Collider2D overlapCollider = Physics2D.OverlapBox( groundChecker.position, groundCheckerDimension, 0, groundMask );

        bool wasGrounded = isGrounded;
        isGrounded = overlapCollider != null;

        if( wasGrounded != isGrounded && !isGrounded )
        {
            StartCoroutine( CoyoteTime() );
        }
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        currentState.OnTriggerEnter();
    }

#if UNITY_EDITOR
    public void OnGUI()
    {
        GUILayout.Label( currentState.ToString() );
        GUILayout.Label( $" Current Speed : {currentSpeed.ToString("0.0")}" );
        GUILayout.Label( $" Max Speed : {currentMaxSpeed.ToString("0.0")}" );
        GUILayout.Label( $" CanJump : {CanJump}" );
    }
#endif

    private void OnDrawGizmos()
    {
        if( isGrounded )
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawCube( groundChecker.position, groundCheckerDimension );
    }

    public void ChangeState( string stateName )
    {
        currentState?.OnExit();
        currentState = _states[ stateName ];
        currentState.OnEnter();
    }

    #region PhysX

    public void HorizontalControl()
    {
        float maxSpeedChange = 0f;
        float targetVelocity = _moveDirection * currentMaxSpeed;

        if ( IsMoving )
        {
            maxSpeedChange = acceleration * Time.deltaTime;
        }
        else
        {
            maxSpeedChange = deceleration * Time.deltaTime;
        }

        currentSpeed = Mathf.MoveTowards( rb2d.linearVelocityX, targetVelocity, maxSpeedChange );
    }

    public void Dash()
    {
        float dashDirection = _moveDirection;
        if( _moveDirection == 0f && _verticalDirection == 0 )
        {
            dashDirection = transform.localScale.x;
        }

        currentSpeed = dashDirection * dashSpeed;
        rb2d.linearVelocityY = _verticalDirection * dashSpeed;
    }

    IEnumerator CoyoteTime()
    {
        Debug.Log( "COYOTE" );
        {
            InCoyoteTime = true;
            yield return new WaitForSeconds( coyoteTimeDuration );
        }

        InCoyoteTime = false;
        Debug.Log( "NOYOTE" );
    }

    IEnumerator JumpBuffering()
    {
        jumpBuffer = true;
        yield return new WaitForSeconds( jumpBufferDuration );
        jumpBuffer = false;
    }

    #endregion

    #region Input Management 
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
                Vector2 direction = context.ReadValue<Vector2>();
                _moveDirection = direction.x;
                _verticalDirection = direction.y;
                break;
            case InputActionPhase.Canceled:
                _moveDirection = 0f;
                _verticalDirection = 0f;
                break;
            default:
                break;
        }
    }

    public void OnJump( InputAction.CallbackContext context )
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
                jumpBtnPressed = true;
                    if ( !jumpBuffer )
                    {
                        StartCoroutine( JumpBuffering() );
                    }
                break;
            case InputActionPhase.Canceled:
                jumpBtnPressed = false;
                break;
            default:
                break;
        }
    }

    public void OnDash( InputAction.CallbackContext context )
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
                dashPressed = true;
                break;
            case InputActionPhase.Canceled:
                dashPressed = false;
                break;
            default:
                break;
        }
    }

    #endregion
}
