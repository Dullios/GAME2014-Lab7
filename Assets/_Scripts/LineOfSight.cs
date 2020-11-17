using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineOfSight : MonoBehaviour
{
    public Collider2D collidesWith;
    public ContactFilter2D contactFilter;
    public List<Collider2D> colliders;

    private BoxCollider2D LoSCollider;

    // Start is called before the first frame update
    void Start()
    {
        LoSCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Physics2D.GetContacts(LoSCollider, contactFilter, colliders);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collidesWith = collision;
    }
}
