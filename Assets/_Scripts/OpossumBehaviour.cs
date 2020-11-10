using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumBehaviour : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public float runForce;
    
    public Transform lookAheadPoint;
    public LayerMask collisionGroundLayer;
    public bool isGroundAhead;

    public Transform lookForwardPoint;
    public LayerMask collisionWallLayer;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LookForward();
        LookAhead();
        Move();
    }

    private void LookForward()
    {
        if (Physics2D.Linecast(transform.position, lookForwardPoint.position, collisionWallLayer))
            FlipX();

        Debug.DrawLine(transform.position, lookForwardPoint.position, Color.red);
    }

    private void LookAhead()
    {
        isGroundAhead = Physics2D.Linecast(transform.position, lookAheadPoint.position, collisionGroundLayer);

        Debug.DrawLine(transform.position, lookAheadPoint.position, isGroundAhead ? Color.green : Color.red);
    }

    private void Move()
    {
        if (isGroundAhead)
        {
            rigidBody2D.AddForce(Vector2.left * runForce * Time.deltaTime * transform.localScale.x);

            rigidBody2D.velocity *= 0.90f;
        }
        else
        {
            FlipX();
        }    
    }

    private void FlipX()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
