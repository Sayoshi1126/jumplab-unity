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
    float px, py;
    [SerializeField] float vx, vy;
    public Vector2 moveInput;
    float pvx, pvy;
    float ay;
    int dir;
    int lastDir;
    int jumpDir;
    public bool Jumping;
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

    BoxCollider2D boxCollider2d;
    float boxColliderX;
    bool corner_correction = false;

    [SerializeField] private ContactFilter2D filter2d = default;

    LineRenderer line;
    int count;
    

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

        pvx = vx;
        pvy = vy;
        px = x;
        py = y;

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
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        rigidbody2D.AddForce(new Vector2(0, jumpPower));
        //        anim.SetTrigger("Jump");
        //    }
            //Grounded = true; // 地面に触れたことにする
            anim.SetBool("Ground", true);

            
            onObstacle = true;
            //if(lastOnObstacle!=onObstacle || onObstacle && Jumping)
            //{
            //    Jumping = false;
            //}
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
        //if (x != 0)
        //{
        //    rigidbody2D.velocity = new Vector2(x * walkSpeed, rigidbody2D.velocity.y);
        //    Vector2 temp = transform.localScale;//localScaleが-1だと逆向き
        //    temp.x = x;
        //    transform.localScale = temp;//現在の向いてる向きをlocalscaleに代入
        //    anim.SetBool("running", true);
        //}
        //else
        //{
        //    rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        //    anim.SetBool("running", false);
        //}

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

        if (wall)
        {
            wallSlide();
        }

        moveInput = new Vector2(vx,vy);
        anim.SetBool("allowAerialJump",Settings.Instance.allowAerialJump);
    }

    private void FixedUpdate()
    {
        float offSet = Settings.Instance.collisionTolerance;
        float penalty = 0;
        Ray2D rayRight = new Ray2D((Vector2)transform.position + Vector2.up * 0.3f + offSet * Vector2.left, Vector2.right);
        Ray2D rayLeft = new Ray2D((Vector2)transform.position + Vector2.up * 0.3f + offSet * Vector2.right, Vector2.left);
        RaycastHit2D hitCastRight = Physics2D.Raycast(rayRight.origin, rayRight.direction, offSet * 2);
        RaycastHit2D hitCastLeft = Physics2D.Raycast(rayLeft.origin, rayLeft.direction, offSet * 2);
        Debug.DrawRay(rayRight.origin, (offSet * 2) * rayRight.direction, Color.red);
        Debug.DrawRay(rayLeft.origin, (offSet*2) * rayLeft.direction, Color.blue);
        if(hitCastLeft.distance==0&&hitCastRight.distance>0)
        {
            penalty = -((offSet + boxColliderX / 2) - hitCastRight.distance)-0.1f;
            corner_correction = true;
        }else if (hitCastRight.distance == 0&&hitCastLeft.distance>0)
        {
            penalty = (offSet + boxColliderX / 2) - hitCastLeft.distance+0.1f;
            corner_correction = true;
            //Debug.Log(hitCastLeft.distance);
        }

        if(corner_correction == true&&Jumping&&!onObstacle)
        {
            transform.position = new Vector2(transform.position.x+penalty,transform.position.y);
        }

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
    }

    void velocityYUpdate()
    {
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

    void judgeShortJump()
    {
        if(!ControllerManager.Instance.CheckButtonDown(ControllerManager.func.jump)&&Input.GetKey(KeyCode.Space)==false)
        {
            shortjump = true;
        }
    }
    void jumpStart()
    {
        anim.SetFloat("jumpAnticipationFrames",Settings.Instance.jumpAnticipationFrames);
        if(!Jumping)
        {
            if(onObstacle&&!wall)
            {
                anim.SetTrigger("Jump");
            }
            else if (!onObstacle&&wall)
            {
                anim.SetTrigger("Jump");
            }
        }
    }
    void jump()
    {
        Jumping = true;
        Settings.Instance.jumping = Jumping;
        if(Settings.Instance.maxPropellingFrames>0)
        {
            propelling = true; 
        }
        jumpDir = lastDir;
        if (shortjump)
        {
            rigidbody2D.AddForce(new Vector2(0, 100 + Mathf.Abs(vx) * Settings.Instance.jumpVelocityBonus));
            shortjump = false;
        }
        else
        {
            rigidbody2D.AddForce(new Vector2(0, Settings.Instance.jumpVelocity + Mathf.Abs(vx) * Settings.Instance.jumpVelocityBonus));
        }
        vx = Settings.Instance.vxAdjustmentAtTakeoff * vx;
    }
    void wallJump()
    {
        if (Settings.Instance.allowWallJump)
        {
            Jumping = true;
            Settings.Instance.jumping = Jumping;
            transform.localScale = new Vector2(-transform.localScale.x, 1);
            rigidbody2D.velocity = new Vector2(Settings.Instance.maxVx * Settings.Instance.wallJumpSpeedRatio * transform.localScale.x, 0);
            Debug.Log(transform.localScale.x);
            rigidbody2D.AddForce(new Vector2(0, Settings.Instance.jumpVelocity + Mathf.Abs(vx) * Settings.Instance.jumpVelocityBonus));
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
            jumpCanceled();
            propelling = false;
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
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,rigidbody2D.velocity.y*0.7f);
    }
}
