using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IApplyDamage
{
    public float verticalSpeed;
    public float verticalBoundary;
    public int damage;

    public ContactFilter2D contactFilter;
    public List<Collider2D> colliders;
    private BoxCollider2D boxCollider;


    public Vector3 direction;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        _CheckBounds();
        CheckCollision();
    }

    private void CheckCollision()
    {
        Physics2D.GetContacts(boxCollider, contactFilter, colliders);

        if(colliders.Count > 0 && colliders[0] != null)
        {
            BulletManager.Instance().ReturnBullet(gameObject);
        }
    }

    private void _Move()
    {
        transform.position += direction * verticalSpeed * Time.deltaTime; //new Vector3(0.0f, verticalSpeed, 0.0f) * Time.deltaTime;
    }

    private void _CheckBounds()
    {
        if (transform.position.y > verticalBoundary)
        {
            BulletManager.Instance().ReturnBullet(gameObject);
        }
    }

    //public void OnTriggerEnter2D(Collider2D other)
    //{
    //    switch(other.gameObject.tag)
    //    {
    //        case "Enemy":
    //            BulletManager.Instance().ReturnBullet(gameObject);
    //            break;
    //        case "Player":
    //            BulletManager.Instance().ReturnBullet(gameObject);
    //            break;
    //    }
    //}

    public int ApplyDamage()
    {
        return damage;
    }
}
