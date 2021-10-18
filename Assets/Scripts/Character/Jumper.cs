using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    public float walkSpeed = 2;
    public float jumpPower = 20;
    private Animator anim;
    float x = 0;
    float y = 0;
    float ay = 0;
    [SerializeField] float vx, vy;
    public Vector2 moveInput;
    int dir;
    int lastDir;
    public bool Jumping;
    bool wallJumping;
    bool enemyJumping;
    public bool onObstacle;
    bool lastOnObstacle;
    public bool propelling;
    public bool standing;
    public bool wall;

    int propellingRemainingFrames;
    float pattern;
    int runningMotionMax;

    //Settings settings;
    static int w = 24;
    static int h = 48;

    float propellingTime;
    float wallTime;

    bool shortjump;
    bool enemyOnStep;

    BoxCollider2D boxCollider2d;
    float boxColliderX;
    bool corner_correction = false;

    [SerializeField] private Transform shotPosition;
    [SerializeField] private ContactFilter2D filter2d = default;
    public GameObject bulletPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        boxColliderX = boxCollider2d.size.x;
    }

    // Update is called once per frame
    void Update()
    {

        control();
        velocityXUpdate();
        velocityYUpdate();

        rigidbody2D.velocity = new Vector2(vx, rigidbody2D.velocity.y);

        anim.SetFloat("x", rigidbody2D.velocity.x);
        anim.SetFloat("y", rigidbody2D.velocity.y);
        
        
        bool ground = rigidbody2D.IsTouching(filter2d);
        x = Input.GetAxisRaw("Horizontal");
        if (ground) // レイが地面に触れたら、
        {
            //Grounded = true; // 地面に触れたことにする
            anim.SetBool("Ground", true);

            
            onObstacle = true;
        }
        else 
        {
            //Grounded = false; // 地面に触れてないことにする
            if (y <= 0)
            {
                anim.SetBool("Ground", false);
            }
            //Jumping = true;
            onObstacle = false;
        }

        lastOnObstacle = onObstacle;

        if (rigidbody2D.velocity.y < 0)
        {
            anim.SetBool("isFalling", true);
            rigidbody2D.gravityScale = Settings.Instance.gravityFalling;
        }
        else
        {
            if(propelling)
            {
                rigidbody2D.gravityScale = 0;
            }
            else
            {
                rigidbody2D.gravityScale = Settings.Instance.gravityRising;
            }
            anim.SetBool("isFalling", false);
        }

        if (wall&&rigidbody2D.velocity.y<0)
        {
            wallSlide();
            //anim.SetBool("wallSlide", true);
        }

        if(Jumping == false)
        {
            wallJumping = false;
            enemyJumping = false;
        }

        moveInput = new Vector2(vx,vy);
        anim.SetBool("allowAerialJump",Settings.Instance.allowAerialJump);
    }

    private void FixedUpdate()
    {
        collisionTorelance();

        wall = false;
        anim.SetBool("wallSlide", false);

        Settings.Instance.jumperX = transform.position.x;
        Settings.Instance.jumperY = transform.position.y;
        Settings.Instance.jumperVX = rigidbody2D.velocity.x;
        Settings.Instance.jumperVY = rigidbody2D.velocity.y;
    }
    void velocityXUpdate()
    {
        float ax;
        float lastFramesVx = vx;
        vx = rigidbody2D.velocity.x;
        if (Jumping||(!onObstacle && !Settings.Instance.allowAerialWalk))
        {
            ax = Settings.Instance.axJumping;
        }else if(Mathf.Sign(vx)!=dir)
        {
            ax = Settings.Instance.axBrake;
        }
        else
        {
            ax = Settings.Instance.axNormal;
        }
        if(dir!=0)
        {
            if (Jumping && !onObstacle&& !Settings.Instance.allowAerialTurn)
            {
                //allowAerialTurn = true
            }
            else
            {
                transform.localScale = new Vector3(dir, transform.localScale.y);
            }
            anim.SetBool("running", true);
            vx += ax * dir;
            if (Settings.Instance.vxAdjustmentAtTakeoff > 0 && Jumping)
            {
                vx = Mathf.Clamp(vx, -Settings.Instance.maxVx * (1 + Settings.Instance.vxAdjustmentAtTakeoff), Settings.Instance.maxVx * (1 + Settings.Instance.vxAdjustmentAtTakeoff));
            }else
            {
                vx = Mathf.Clamp(vx, -Settings.Instance.maxVx, Settings.Instance.maxVx);
            }

        }
        else
        {
            anim.SetBool("running", false);
            vx -= ax * Mathf.Sign(vx);
            if (Mathf.Abs(vx) <= ax)
            {
                vx = 0;
            }
        }

        if(Settings.Instance.stopAndFall&&!onObstacle&&!Jumping)
        {
            vx = 0;
            anim.SetBool("running",false);
        }

        vx = Mathf.Clamp(vx, -Settings.Instance.maxVx, Settings.Instance.maxVx);
        ax = (vx - lastFramesVx) / Time.deltaTime;
        Settings.Instance.jumperAX = ax;
    }

    void velocityYUpdate()
    {
        float ay;
        float lastFramesVy = vy;
        vy = rigidbody2D.velocity.y;

        if(Jumping == true && propelling == true&&propellingTime<=Settings.Instance.maxPropellingFrames)
        {
            propellingTime += Time.deltaTime;
        }else if (Jumping&&propelling&&propellingTime > Settings.Instance.maxPropellingFrames)
        {
            propelling = false;
            rigidbody2D.gravityScale = Settings.Instance.gravityFalling;
        }

        if (onObstacle && !Jumping)
        {
            vy = 0;
        }

        if (rigidbody2D.velocity.y < -Settings.Instance.maxVy)
        {
            vy = -Settings.Instance.maxVy;
        }

        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,vy);
        ay = (vy - lastFramesVy) / Time.deltaTime;
        Settings.Instance.jumperAY = ay;
    }


    void move()
    {
        dir = (int)x;//キーの方向
        if (dir != lastDir)
        {
            Settings.Instance.dir = dir;
        }
        if (dir != 0)
        {
            lastDir = dir;
        }
    }
    void jumpStart()
    {
        float jumpAnticipationFrames = Settings.Instance.jumpAnticipationFrames;
        if (jumpAnticipationFrames == 0)
        {
            anim.SetFloat("jumpAnticipationFrames", 100);
        }
        else
        {
            anim.SetFloat("jumpAnticipationFrames", 1/jumpAnticipationFrames);
        }
        if(!Jumping)
        {
            if (onObstacle && !wall)
            {
                anim.SetTrigger("Jump");
            }
            else if (!onObstacle && wall)
            {
                anim.SetTrigger("Jump");
            }
            else if (enemyOnStep)
            {
                anim.SetTrigger("EnemyJump");
            }
        }
    }
    void jump()
    {
        float jumpPower;
        if (enemyOnStep)
        {
            jumpPower = Settings.Instance.enemyStepVelocity;
            enemyOnStep = false;
            enemyJumping = true;
            Debug.Log("enemyJump");
        }
        else
        {
            jumpPower = Settings.Instance.jumpVelocity;
        }
        Jumping = true;
        Settings.Instance.jumping = Jumping;
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        collisionTorelance();
        if(Settings.Instance.maxPropellingFrames>0)
        {
            propelling = true; 
        }
        
        rigidbody2D.AddForce(new Vector2(0, jumpPower + Mathf.Abs(vx) * Settings.Instance.jumpVelocityBonus));
        
        vx = Settings.Instance.vxAdjustmentAtTakeoff * vx;
    }

    void shot()
    {
        Debug.Log("shot");
        Instantiate(bulletPrefab,shotPosition.transform.position,transform.rotation);
    }

    void wallJump()
    {
        if (Settings.Instance.allowWallJump)
        {
            transform.localScale = new Vector2(-transform.localScale.x, 1);
            rigidbody2D.velocity = new Vector2(Settings.Instance.maxVx * Settings.Instance.wallJumpSpeedRatio * transform.localScale.x, 0);
            rigidbody2D.AddForce(new Vector2(0, Settings.Instance.jumpVelocity + Mathf.Abs(vx) * Settings.Instance.jumpVelocityBonus));
            Jumping = true;
            wallJumping = true;
            Settings.Instance.jumping = Jumping;
        }
    }
    void jumpCanceled()
    {
        if (Jumping)
        {
            ay = Settings.Instance.gravityFalling;
            if (vy > 0) rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y * Settings.Instance.verticalSpeedSustainLevel);
        }
    }

    void control()
    {
        move();
        if(Input.GetKeyDown(KeyCode.Space)|| ControllerManager.Instance.jumpButtonDown)
        {
            jumpStart();
            propellingTime = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Space)|| ControllerManager.Instance.jumpButtonUp)
        {
            propelling = false;
        }
        else if(Jumping&& !Input.GetKey(KeyCode.Space))
        {
            jumpCanceled();
        }
        else if(Jumping&& Input.GetKey(KeyCode.Space)&&enemyJumping&&!Settings.Instance.enemyStepJump)
        {
            jumpCanceled();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            shot();
        }
    }

    void ground()
    {
        Jumping = false;
        Settings.Instance.jumping = Jumping;
        propelling = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "ground"&&!onObstacle&&transform.localScale.x==dir&&Settings.Instance.allowWallSlide)
        {
            wall = true;
        }
    }

    void wallSlide()
    {
        anim.SetBool("wallSlide",true);
        Jumping = false;

        Settings.Instance.jumping = Jumping;
        propelling = false;
        if (rigidbody2D.velocity.y < -Settings.Instance.maxVy * 0.2f)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -Settings.Instance.maxVy * 0.2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        float judgePos = transform.position.y+0.1f;

        foreach(ContactPoint2D p in collision.contacts)
        {
            if (p.point.y<judgePos)
            {
                if (collision.gameObject.tag == "enemy"&&Settings.Instance.enemyStep)
                {
                    Debug.Log(collision.gameObject.tag);
                    Jumping = false;
                    enemyOnStep = true;
                    jumpStart();
                }
            }
            else
            {

            }
        }
    }

    void collisionTorelance()
    {
        float offSet = 0.1f;
        float toleranceLength = Settings.Instance.collisionTolerance;
        float penalty = 0;
        Ray2D rayRight = new Ray2D((Vector2)transform.position + Vector2.up * 0.7f + offSet * Vector2.right, Vector2.left);//右のRay
        Ray2D rayLeft = new Ray2D((Vector2)transform.position + Vector2.up * 0.7f + offSet * Vector2.left, Vector2.right);//左のRay

        RaycastHit2D hitCastLeft = Physics2D.Raycast(rayLeft.origin, rayLeft.direction, offSet * 2);
        RaycastHit2D hitCastRight = Physics2D.Raycast(rayRight.origin, rayRight.direction, offSet * 2);
        //RaycastHit2D hitCastLeft = Physics2D.Raycast(rayLeft.origin, rayLeft.direction, offSet * 2);
        Debug.DrawRay(rayRight.origin + Vector2.up * 0.01f, (toleranceLength) * rayRight.direction, Color.red);
        Debug.DrawRay(rayLeft.origin, (toleranceLength) * rayLeft.direction, Color.blue);

        corner_correction = false;

        bool moveChara = false;
        if (hitCastRight.distance < Settings.Instance.collisionTolerance && hitCastLeft.distance > 0)//右方向のRay(左サイド)だけ始点が壁にめり込んでない
        {
            corner_correction = true;
            penalty = -(offSet * 2 - hitCastLeft.distance);
            if (-penalty < toleranceLength)
            {
                Debug.Log("hitRightSide");
                moveChara = true;
            }
        }
        else if (hitCastLeft.distance < Settings.Instance.collisionTolerance && hitCastRight.distance > 0)
        {
            corner_correction = true;
            penalty = offSet * 2 - hitCastRight.distance;
            if (penalty < toleranceLength)
            {
                Debug.Log("hitLeftSide");
                moveChara = true;
            }
        }

        if (corner_correction == true && Jumping)// && !onObstacle)
        {
            if (moveChara == true)
            {
                if(penalty<0)
                {
                    penalty -= 0.05f;
                }
                else
                {
                    penalty += 0.05f;
                }
                Debug.Log("collisionTorelance");
                transform.position = new Vector2(transform.position.x + penalty, transform.position.y);
            }
        }
    }
}
