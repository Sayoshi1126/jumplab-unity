using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    [HideInInspector] float vx, vy;
    public new Rigidbody2D rigidbody2D;
    private Collider2D collider;
    public float walkSpeed = 2;
    public bool deathTrigger;

    [SerializeField] bool edgeTurn;
    [SerializeField] bool wallTurn;

    [SerializeField] Animator enemyAnimator;
    


    void Start()
    {
        rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        collider = this.gameObject.GetComponent<Collider2D>();
        enemyAnimator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        velocityXUpdate();
    }

    private void FixedUpdate()
    {
        float offSet = 0.1f;
        Ray2D rayFront = new Ray2D((Vector2)transform.position + Vector2.up*0.32f+Vector2.right*0.16f*transform.localScale.x, Vector2.right*transform.localScale.x);
        Ray2D rayRight = new Ray2D((Vector2)transform.position  + offSet * Vector2.right, Vector2.up*-1);
        Ray2D rayLeft = new Ray2D((Vector2)transform.position + offSet * Vector2.left, Vector2.up*-1);

        RaycastHit2D hitCastFront = Physics2D.Raycast(rayFront.origin, rayFront.direction, offSet);
        RaycastHit2D hitCastRight = Physics2D.Raycast(rayRight.origin, rayRight.direction, offSet * 2);
        RaycastHit2D hitCastLeft = Physics2D.Raycast(rayLeft.origin, rayLeft.direction, offSet * 2);

        Debug.DrawRay(rayFront.origin, offSet * rayFront.direction, Color.yellow);
        Debug.DrawRay(rayRight.origin, (offSet * 2) * rayRight.direction, Color.red);
        Debug.DrawRay(rayLeft.origin, (offSet * 2) * rayLeft.direction, Color.blue);

        if (edgeTurn)
        {
            if (hitCastLeft.distance == 0 && hitCastRight.distance != 0 && hitCastRight.collider.tag == "ground")
            {
                transform.localScale = new Vector2(1, transform.localScale.y);
            }
            else if (hitCastLeft.distance != 0 && hitCastRight.distance == 0 && hitCastLeft.collider.tag == "ground")
            {
                transform.localScale = new Vector2(-1, transform.localScale.y);
            }
        }

        if (wallTurn)
        {
            if (hitCastFront.distance != 0 && hitCastFront.collider.tag == "ground")
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
        }

        if(deathTrigger)
        {
            Death();
        }
    }

    void velocityXUpdate()
    {
        if(deathTrigger==true)
        {
            vx = 0;
        }
        else
        {
            vx = walkSpeed;
        }
        rigidbody2D.velocity = new Vector2(vx*transform.localScale.x,rigidbody2D.velocity.y);
    }

    void Death()
    {
        enemyAnimator.SetTrigger("deathTrigger");
        StartCoroutine(DelayElimination());
    }

    private IEnumerator DelayElimination()
    {
        gameObject.layer = 11;
        // 10フレーム待つ
        for (var i = 0; i < 20; i++)
        {
            yield return null;
        }

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        deathTrigger = false;
        StartCoroutine(DelayBirth());

    }

    private IEnumerator DelayBirth()
    {
        for (var i = 0; i < 300; i++)
        {
            yield return null;
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.layer = 10;
    }
}
