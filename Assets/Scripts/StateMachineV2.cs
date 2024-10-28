using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineV2 : MonoBehaviour
{

    public enum StateV2
    {
        YELLOW,WHITE,BLUE
    }

    public StateV2 currentState;

    public SpriteRenderer image;

    private float chrono = 0f;
    private bool spacePressed = false;

    private void OnEnable()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TransitionToState( StateV2.WHITE );
    }

    // Update is called once per frame
    void Update()
    {
        OnStateUpdate();
    }

    private void OnStateEnter()
    {
        switch ( currentState )
        {
            case StateV2.YELLOW:
                image.color = Color.yellow;
                break;
            case StateV2.WHITE:
                image.color = Color.white;
                break;
            case StateV2.BLUE:
                image.color = Color.blue;
                break;
            default:
                break;
        }
    }

    private void OnStateExit()
    {
        switch ( currentState )
        {
            case StateV2.YELLOW:
                chrono = 0f;
                break;
            case StateV2.WHITE:
                chrono = 0f;
                break;
            case StateV2.BLUE:
                break;
            default:
                break;
        }
    }

    private void OnStateUpdate()
    {
        switch ( currentState )
        {
            case StateV2.YELLOW:
                chrono += Time.deltaTime;

                if ( spacePressed )
                {
                    TransitionToState( StateV2.WHITE );
                }else if ( chrono >= 5f )
                {
                    TransitionToState( StateV2.BLUE );
                }

                    break;
            case StateV2.WHITE:
                chrono += Time.deltaTime;

                if( chrono >= 5f )
                {
                    TransitionToState( StateV2.YELLOW );
                }
                break;
            case StateV2.BLUE:

                 if ( spacePressed )
                {
                    TransitionToState( StateV2.WHITE );
                }
                break;
            default:
                break;
        }
    }

    private void OnStateFixedUpdate()
    {

    }

    private void OnStateTriggerEnter2D( Collider2D collision )
    {
        switch ( currentState )
        {
            case StateV2.YELLOW:

                //collision.CompareTag("Enemy")
                break;
            case StateV2.WHITE:
                break;
            case StateV2.BLUE:
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        OnStateTriggerEnter2D( collision );
    }

    private void TransitionToState( StateV2 newState )
    {
        OnStateExit();
        currentState = newState;
        OnStateEnter();
    }

    public void SpacePress( InputAction.CallbackContext context )
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
                spacePressed = true;
                break;
            case InputActionPhase.Canceled:
                spacePressed = false;
                break;
            default:
                break;
        }
    }
}
