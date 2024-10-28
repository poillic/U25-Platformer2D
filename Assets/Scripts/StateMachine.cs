using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachine : MonoBehaviour
{
    public SpriteRenderer image;

    public enum State
    {
        YELLOW,BLUE,WHITE
    };
    public State currentState = State.WHITE;

    private float chrono = 0f;
    private bool triggered = false;
    private bool spacePressed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        switch ( currentState )
        {
            case State.YELLOW:

                image.color = Color.yellow;
                
                if( triggered )
                {
                    triggered = false;
                    currentState = State.BLUE;
                }
                else if ( spacePressed )
                {
                    currentState = State.WHITE;
                }

                break;
            case State.BLUE:

                image.color = Color.blue;

                if ( spacePressed )
                {
                    currentState = State.WHITE;
                }

                break;
            case State.WHITE:

                image.color = Color.white;
                chrono += Time.deltaTime;

                if ( chrono >= 5f )
                {
                    chrono = 0f;
                    currentState = State.YELLOW;
                }

                break;
            default:
                break;
        }

    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if( currentState == State.YELLOW )
        {
            triggered = true;
        }
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
