using UnityEngine;

public class Jumper : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    public float walkSpeed = 2;
    public float jumpPower = 20;
    private Animator anim;
    private Animator swordAnim;
    [SerializeField] private GameObject swordEffect;
    float x = 0;
    float y = 0;
    float ay = 0;
    [SerializeField] float vx, vy;
    public Vector2 moveInput;
    float dir;
    float lastDir;
    public bool Jumping;
    bool wallJumping;
    bool enemyJumping;
    public bool onObstacle;
    bool lastOnObstacle;
    public bool propelling;
    public bool standing;
    public bool wall;
    public bool canControl = true;

    int propellingRemainingFrames;
    float pattern;
    int runningMotionMax;
    float runAnimationSpeed;

    //Settings settings;
    static int w = 24;
    static int h = 48;

    float propellingTime;
    float wallTime;
    int countingCoyoteTime;

    bool shortjump;
    bool enemyOnStep;

    BoxCollider2D boxCollider2d;
    float boxColliderX;
    bool corner_correction = false;

    [SerializeField] private Transform shotPosition;
    [SerializeField] private ContactFilter2D filter2d = default;
    public GameObject bulletPrefab;
    public GameObject swordObject;
    public GameObject swordHitbox;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        swordAnim = swordEffect.GetComponent<Animator>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        boxColliderX = boxCollider2d.size.x;
        countingCoyoteTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        control();
        //velocityXUpdate();
        //velocityYUpdate();

        rigidbody2D.velocity = new Vector2(vx, rigidbody2D.velocity.y);

        anim.SetFloat("x", rigidbody2D.velocity.x);
        anim.SetFloat("runningAnimSpeed", Mathf.Abs(rigidbody2D.velocity.x) + 0.3f);
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
            rigidbody2D.gravityScale = Settings.Instance.jumpParam.gravityFalling;
        }
        else
        {
            if (propelling)
            {
                rigidbody2D.gravityScale = 0;
            }
            else
            {
                rigidbody2D.gravityScale = Settings.Instance.jumpParam.gravityRising;
            }
            anim.SetBool("isFalling", false);
        }

        if (wall && rigidbody2D.velocity.y < 0)
        {
            wallSlide();
            //anim.SetBool("wallSlide", true);
        }

        if (Jumping == false)
        {
            wallJumping = false;
            enemyJumping = false;
        }

        moveInput = new Vector2(vx, vy);
        anim.SetBool("allowAerialJump", Settings.Instance.jumpParam.allowAerialJump);
    }

    private void FixedUpdate()
    {
        //control();
        velocityXUpdate();
        velocityYUpdate();

        collisionTorelance();

        wall = false;
        anim.SetBool("wallSlide", false);

        if (!onObstacle)
        {
            countingCoyoteTime += 1;
        }
        else
        {
            countingCoyoteTime = 0;
        }

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
        if (Jumping || (!onObstacle && !Settings.Instance.jumpParam.allowAerialWalk))
        {
            ax = Settings.Instance.jumpParam.axJumping;
        }
        else if ((Mathf.Sign(vx)*dir==-1&vx!=0))
        {
            ax = Settings.Instance.jumpParam.axBrake;
        }
        else
        {
            ax = Settings.Instance.jumpParam.axNormal;
        }
        if (dir != 0)
        {
            anim.SetBool("running", true);
            if (Mathf.Abs(vx) < Settings.Instance.jumpParam.maxVx || vx * dir < 0)
            {
                vx += ax * dir;
                vx = Mathf.Clamp(vx, -Settings.Instance.jumpParam.maxVx, Settings.Instance.jumpParam.maxVx);
            }
            //if (Settings.Instance.jumpParam.vxAdjustmentAtTakeoff > 0 && Jumping)
            //{
            //    vx = Mathf.Clamp(vx, -Settings.Instance.jumpParam.maxVx * (1 + Settings.Instance.jumpParam.vxAdjustmentAtTakeoff), Settings.Instance.jumpParam.maxVx * (1 + Settings.Instance.jumpParam.vxAdjustmentAtTakeoff));
            //}else
            //{
            //    vx = Mathf.Clamp(vx, -Settings.Instance.jumpParam.maxVx, Settings.Instance.jumpParam.maxVx);
            //}
        }
        else
        {
            anim.SetBool("running", false);
            if (Settings.Instance.jumpParam.aerialInertia && !onObstacle && Jumping)
            {
                ax = 0;
            }
            vx -= ax * Mathf.Sign(vx);
            if (Mathf.Abs(vx) <= ax)
            {
                vx = 0;
            }
        }

        if (dir != 0)
        {
            if (Jumping && !onObstacle && !Settings.Instance.jumpParam.allowAerialTurn)
            {
                //allowAerialTurn = true
            }
            else
            {
                if (dir > 0) transform.localScale = new Vector3(1, transform.localScale.y);
                else if (dir < 0) transform.localScale = new Vector3(-1, transform.localScale.y);
            }
        }

        if (Settings.Instance.jumpParam.stopAndFall && !onObstacle && !Jumping)
        {
            vx = 0;
            anim.SetBool("running", false);
        }

        //vx = Mathf.Clamp(vx, -Settings.Instance.jumpParam.maxVx, Settings.Instance.jumpParam.maxVx);
        ax = (vx - lastFramesVx) / Time.deltaTime;
        Settings.Instance.jumperAX = ax;


    }

    void velocityYUpdate()
    {
        float ay;
        float lastFramesVy = vy;
        vy = rigidbody2D.velocity.y;

        if (Jumping == true && propelling == true && propellingTime <= Settings.Instance.jumpParam.maxPropellingFrames)
        {
            propellingTime += Time.deltaTime;
        }
        else if (Jumping && propelling && propellingTime > Settings.Instance.jumpParam.maxPropellingFrames)
        {
            propelling = false;
            rigidbody2D.gravityScale = Settings.Instance.jumpParam.gravityFalling;
        }

        if (onObstacle && !Jumping)
        {
            vy = 0;
        }

        if (rigidbody2D.velocity.y < -Settings.Instance.jumpParam.maxVy)
        {
            vy = -Settings.Instance.jumpParam.maxVy;
        }

        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, vy);
        ay = (vy - lastFramesVy) / Time.deltaTime;
        Settings.Instance.jumperAY = ay;
    }


    void move()
    {
        dir = x;//キーの方向
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
        float jumpAnticipationFrames = Settings.Instance.jumpParam.jumpAnticipationFrames;
        if (jumpAnticipationFrames == 0)
        {
            anim.SetFloat("jumpAnticipationFrames", 100);
        }
        else
        {
            anim.SetFloat("jumpAnticipationFrames", 1 / jumpAnticipationFrames);
        }
        if (!Jumping)
        {
            if (onObstacle && !wall)
            {
                anim.SetTrigger("Jump");
            }
            else if (!onObstacle && wall)
            {
                anim.SetTrigger("Jump");
            }
            else if (countingCoyoteTime < Settings.Instance.jumpParam.coyoteTime)
            {
                Debug.Log(countingCoyoteTime);
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
            Settings.Instance.attackParam.enemyStepVelocity = Settings.Instance.jumpParam.jumpVelocity; // 実験用
            jumpPower = Settings.Instance.attackParam.enemyStepVelocity;
            enemyOnStep = false;
            enemyJumping = true;
        }
        else
        {
            jumpPower = Settings.Instance.jumpParam.jumpVelocity;
        }
        Jumping = true;
        Settings.Instance.jumping = Jumping;
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        collisionTorelance();
        if (Settings.Instance.jumpParam.maxPropellingFrames > 0)
        {
            propelling = true;
        }
        rigidbody2D.AddForce(new Vector2(Settings.Instance.jumpParam.vxAdjustmentAtTakeoff * vx, jumpPower + Mathf.Abs(vx) * Settings.Instance.jumpParam.jumpVelocityBonus));
    }

    void shot()
    {
        Debug.Log("shot");
        anim.SetTrigger("Shot");
        if (GameManager.Instance.bulletNum < Settings.Instance.attackParam.bulletLimit)
        {
            Instantiate(bulletPrefab, shotPosition.transform.position, transform.rotation);
        }
    }

    void wallJump()
    {
        if (Settings.Instance.jumpParam.allowWallJump)
        {
            transform.localScale = new Vector2(-transform.localScale.x, 1);
            rigidbody2D.velocity = new Vector2(Settings.Instance.jumpParam.maxVx * Settings.Instance.jumpParam.wallJumpSpeedRatio * transform.localScale.x, 0);
            rigidbody2D.AddForce(new Vector2(0, Settings.Instance.jumpParam.jumpVelocity + Mathf.Abs(vx) * Settings.Instance.jumpParam.jumpVelocityBonus));
            Jumping = true;
            wallJumping = true;
            Settings.Instance.jumping = Jumping;
        }
    }
    void jumpCanceled()
    {
        if (Jumping)
        {
            //ay = Settings.Instance.jumpParam.gravityFalling;
            if (vy > 0) rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y * Settings.Instance.jumpParam.verticalSpeedSustainLevel);
        }
    }

    void control()
    {
        move();
        if (canControl)
        {
            if (Input.GetKeyDown(KeyCode.Space) || ControllerManager.Instance.jump.WasPressed)
            {
                jumpStart();
                propellingTime = 0;
            }
            else if (Input.GetKeyUp(KeyCode.Space) || ControllerManager.Instance.jump.WasReleased)
            {
                propelling = false;
            }
            else if (Jumping && !Input.GetKey(KeyCode.Space) && !ControllerManager.Instance.jump.IsPressed)
            {
                jumpCanceled();
            }
            else if (Jumping && Input.GetKey(KeyCode.Space) && enemyJumping && !Settings.Instance.attackParam.enemyStepJump || Jumping && ControllerManager.Instance.jump.IsPressed && enemyJumping && !Settings.Instance.attackParam.enemyStepJump)
            {
                jumpCanceled();
            }

            if (Input.GetKeyDown(KeyCode.Z) || ControllerManager.Instance.bullet.WasPressed)
            {
                shot();
            }

            if (Input.GetKeyDown(KeyCode.X) || ControllerManager.Instance.cut.WasPressed)
            {
                sword();
            }
        }
        else
        {
            x = 0;
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
        if (collision.tag == "ground" && !onObstacle && transform.localScale.x == dir && Settings.Instance.jumpParam.allowWallSlide)
        {
            wall = true;
        }
    }

    void wallSlide()
    {
        anim.SetBool("wallSlide", true);
        Jumping = false;

        Settings.Instance.jumping = Jumping;
        propelling = false;
        if (rigidbody2D.velocity.y < -Settings.Instance.jumpParam.maxVy * 0.2f)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -Settings.Instance.jumpParam.maxVy * 0.2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        float judgePos = transform.position.y + 0.1f;

        foreach (ContactPoint2D p in collision.contacts)
        {
            if (p.point.y < judgePos)
            {
                if (collision.gameObject.tag == "enemy" && Settings.Instance.attackParam.enemyStep)
                {
                    var enemyScript = collision.gameObject.GetComponent<Enemy>();
                    enemyScript.deathTrigger = true;
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

    void sword()
    {
        anim.SetTrigger("Saber");
        swordAnim.SetTrigger("cut");
        swordAnim.SetFloat("frameStartup", 7 / (float)Settings.Instance.attackParam.atackStartupFrame);
        anim.SetFloat("frameStartup", 7 / (float)Settings.Instance.attackParam.atackStartupFrame);
        swordAnim.SetFloat("frameActiveOn", 8 / (float)Settings.Instance.attackParam.atackActiveFrame);
        swordObject.transform.localScale = new Vector3(Settings.Instance.attackParam.atackRange, 1, 1);
        swordObject.transform.localPosition = new Vector3(0.356f * Settings.Instance.attackParam.atackRange, 0.246f, 0);
    }
    void attackStartup()
    {
        //canControl = Settings.Instance.attackParam.walkAttack;
    }
    void attackHitboxStart()
    {
        swordHitbox.SetActive(true);
    }
    void attackHitboxEnd()
    {
        swordHitbox.SetActive(false);
    }

    void atackEnd()
    {
        canControl = true;
    }

    void collisionTorelance()
    {
        float offSet = 0.1f;
        float toleranceLength = Settings.Instance.jumpParam.collisionTolerance;
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
        if (hitCastRight.distance < Settings.Instance.jumpParam.collisionTolerance && hitCastLeft.distance > 0)//右方向のRay(左サイド)だけ始点が壁にめり込んでない
        {
            corner_correction = true;
            penalty = -(offSet * 2 - hitCastLeft.distance);
            if (-penalty < toleranceLength)
            {

                moveChara = true;
            }
        }
        else if (hitCastLeft.distance < Settings.Instance.jumpParam.collisionTolerance && hitCastRight.distance > 0)
        {
            corner_correction = true;
            penalty = offSet * 2 - hitCastRight.distance;
            if (penalty < toleranceLength)
            {

                moveChara = true;
            }
        }

        if (corner_correction == true && Jumping)// && !onObstacle)
        {
            if (moveChara == true)
            {
                if (penalty < 0)
                {
                    penalty -= 0.05f;
                }
                else
                {
                    penalty += 0.05f;
                }
                Debug.Log(hitCastRight.distance + "," + hitCastLeft.distance);
                transform.position = new Vector2(transform.position.x + penalty, transform.position.y);
            }
        }
    }
}
