using UnityEngine;

public class NoGravity : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 new_vel = new Vector2( 15f, GetComponent<Rigidbody2D>().linearVelocity.y );

        GetComponent<Rigidbody2D>().linearVelocity = new_vel;
    }
}
