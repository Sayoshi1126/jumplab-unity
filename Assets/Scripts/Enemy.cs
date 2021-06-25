using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float vx, vy;
    public new Rigidbody2D rigidbody2D;
    public float walkSpeed = 2;
    


    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        velocityXUpdate();
    }

    private void FixedUpdate()
    {
        float offSet = 0.1f;
        Ray2D rayRight = new Ray2D((Vector2)transform.position  + offSet * Vector2.right, Vector2.up*-1);
        Ray2D rayLeft = new Ray2D((Vector2)transform.position + offSet * Vector2.left, Vector2.up*-1);
        RaycastHit2D hitCastRight = Physics2D.Raycast(rayRight.origin, rayRight.direction, offSet * 2);
        RaycastHit2D hitCastLeft = Physics2D.Raycast(rayLeft.origin, rayLeft.direction, offSet * 2);
        Debug.DrawRay(rayRight.origin, (offSet * 2) * rayRight.direction, Color.red);
        Debug.DrawRay(rayLeft.origin, (offSet * 2) * rayLeft.direction, Color.blue);

        Debug.Log(hitCastLeft.distance==0);
        if(hitCastLeft.distance == 0 && hitCastRight.distance != 0)
        {
            transform.localScale = new Vector2(1,transform.localScale.y);
        }
        else if(hitCastLeft.distance != 0 && hitCastRight.distance == 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
    }

    void velocityXUpdate()
    {
        vx = walkSpeed;
        rigidbody2D.velocity = new Vector2(vx*transform.localScale.x,rigidbody2D.velocity.y);
    }
}
