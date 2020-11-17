using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RampDirection
{
    NONE,
    UP,
    DOWN
}

public class OpossumBehaviour : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public float runForce;
    
    public Transform lookAheadPoint;
    public LayerMask collisionGroundLayer;
    public bool isGroundAhead;

    public Transform lookForwardPoint;
    public LayerMask collisionWallLayer;

    public bool onRamp;
    public RampDirection rampDir;

    public LineOfSight opossumLoS;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();

        rampDir = RampDirection.NONE;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HasLineOfSight())
            Debug.Log("Opossum can see the player");
        LookForward();
        LookAhead();
        Move();
    }

    private bool HasLineOfSight()
    {
        if (opossumLoS.colliders.Count > 0)
        {
            if (opossumLoS.collidesWith.gameObject.name == "Player" && opossumLoS.colliders[0].gameObject.name == "Player")
                return true;
        }

        return false;
    }

    private void LookForward()
    {
        var wallHit = Physics2D.Linecast(transform.position, lookForwardPoint.position, collisionWallLayer);

        if (wallHit)
        {
            if (!wallHit.collider.CompareTag("Ramps") && !onRamp && transform.localEulerAngles.z == 0)
            {
                FlipX();
                rampDir = RampDirection.DOWN;
            }
            else
                rampDir = RampDirection.UP;
        }

        Debug.DrawLine(transform.position, lookForwardPoint.position, Color.red);
    }

    private void LookAhead()
    {
        var groundHit = Physics2D.Linecast(transform.position, lookAheadPoint.position, collisionGroundLayer);

        if (groundHit)
        {
            if (groundHit.collider.CompareTag("Ramps"))
            {
                onRamp = true;
            }

            if (groundHit.collider.CompareTag("Platforms"))
            {
                onRamp = false;
            }

            isGroundAhead = true;
        }
        else
            isGroundAhead = false;

        Debug.DrawLine(transform.position, lookAheadPoint.position, isGroundAhead ? Color.green : Color.red);
    }

    private void Move()
    {
        if (isGroundAhead)
        {
            rigidBody2D.AddForce(Vector2.left * runForce * Time.deltaTime * transform.localScale.x);

            float upForce;
            if (onRamp)
            {
                if (rampDir == RampDirection.UP)
                    upForce = 0.8f;
                else
                    upForce = 0.2f;
                
                rigidBody2D.AddForce(Vector2.up * (runForce * upForce) * Time.deltaTime * transform.localScale.x);

                StartCoroutine(Rotate());
            }
            else
            {
                StartCoroutine(Normalize());
            }

            rigidBody2D.velocity *= 0.90f;
        }
        else
        {
            FlipX();
        }    
    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.Euler(0, 0, -26);
    }

    IEnumerator Normalize()
    {
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void FlipX()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
