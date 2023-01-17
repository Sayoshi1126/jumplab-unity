using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Settings : SingletonMonoBehaviour<Settings>
{
    [HideInInspector] public float dir;

    [HideInInspector] public float jumperX;
    [HideInInspector] public float jumperY;
    [HideInInspector] public float jumperVX;
    [HideInInspector] public float jumperVY;
    [HideInInspector] public float jumperAX;
    [HideInInspector] public float jumperAY;

    [HideInInspector] public float cameraX;
    [HideInInspector] public float cameraY;
    [HideInInspector] public float cameraVX;
    [HideInInspector] public float cameraVY;

    [HideInInspector] public Rigidbody2D Masaorb2D;
    [HideInInspector] public Jumper jumper;
    [HideInInspector] public GameObject Masao;
    bool preShowTrail;

    [HideInInspector] public bool jumping;

    [Serializable]
    public struct JumpParamater
    {
        [PersistentAmongPlayMode] public bool allowAerialJump;// Allow aerial walk or not.
        [PersistentAmongPlayMode] public int coyoteTime;// Allow aerial jump or not.
        [PersistentAmongPlayMode] public bool allowAerialWalk;// Allow aerial walk or not.
        [PersistentAmongPlayMode] public bool allowAerialTurn; //Allow aerial turn or not.
        [PersistentAmongPlayMode] public bool stopAndFall; // The hoizontal motion is halted when the jumper goes off the foothold.
        [PersistentAmongPlayMode] public bool allowWallJump; // Allow wall jump or not.
        [PersistentAmongPlayMode] public bool allowWallSlide; // Allow wall slide or not.
        [PersistentAmongPlayMode] public bool aerialInertia; // Allow wall slide or not.
        [PersistentAmongPlayMode] public float maxVx; // Maximum horizontal velocity of the jumper.
        [PersistentAmongPlayMode] public float maxVy; // Maximum vertical velocity of the jumper. Limits only the falling motion.
        [PersistentAmongPlayMode] public float jumpVelocity;// Initial vertical velocity of a jump motion.
        [PersistentAmongPlayMode] public float jumpVelocityBonus;// The fasterrun gives an initial jump velocity bonus that allows a higher jump.
        [PersistentAmongPlayMode] public int jumpAnticipationFrames; //Duration of the anticipation of jump motion in frames.
        [PersistentAmongPlayMode] public float vxAdjustmentAtTakeoff; //Horizontal velocity adjustment at the takeoff.
        [PersistentAmongPlayMode] public float maxPropellingFrames;
        [PersistentAmongPlayMode] public float gravityRising;
        [PersistentAmongPlayMode] public float gravityFalling; // gravity when falling.
        [PersistentAmongPlayMode] public float verticalSpeedSustainLevel; // Sustain level of the vertical speed when the button released.
        [PersistentAmongPlayMode] public float axNormal; // Horizontal acceleration in normal state.
        [PersistentAmongPlayMode] public float axBrake; // Horizontal acceleration when braking.
        [PersistentAmongPlayMode] public float axJumping; // Horizontal acceleration when jumping.
        [PersistentAmongPlayMode] public float collisionTolerance; // Tolerance to automatically avoid blocks when jumping (in pixels).
        [PersistentAmongPlayMode] public float wallJumpSpeedRatio; // The velocity raito of the jumping speed of the wall jump to the maxVx.
    }
    [SerializeField] public JumpParamater jumpParam;
    [Serializable]
    public struct AttackParamater
    {
        [PersistentAmongPlayMode] public bool enemyStep;//踏みつけの可否
        [Range(1, 1000)] [PersistentAmongPlayMode] public float enemyStepVelocity;//踏みつけ時のバウンドの強さ
        [PersistentAmongPlayMode] public bool enemyStepJump;//踏みつけジャンプの可否
        //[PersistentAmongPlayMode] public float bulletSpeed = 10f;
        [PersistentAmongPlayMode] public bool UseCurve;
        public AnimationCurve bulletSpeed;
        [Range(0.1f, 30)] [PersistentAmongPlayMode] public float bulletMaxSpeed;
        [Range(0, 30)] [PersistentAmongPlayMode] public float bulletStartSpeed;
        [Range(0, 1)] [PersistentAmongPlayMode] public float bulletAx;
        [Range(0.1f, 5)] [PersistentAmongPlayMode] public float BulletLifeTime;
        [Range(-90, 90)] [PersistentAmongPlayMode] public float shotAngle;
        [Range(0, 10)] [PersistentAmongPlayMode] public int bulletLimit;
        [Range(1, 10)] [PersistentAmongPlayMode] public int atackStartupFrame;
        [Range(1, 10)] [PersistentAmongPlayMode] public int atackActiveFrame;
        [Range(0.2f, 3)] [PersistentAmongPlayMode] public float atackRange;
        [PersistentAmongPlayMode] public bool walkAttack;
    }
    [SerializeField] public AttackParamater attackParam;

    // Camera parameters
    //public bool showCameraMarker = false; // Show the center marker or not.
    //public bool cameraEasing_x = false; // Ease the horizon camera motion or not.
    //public bool cameraEasing_y = true; // Ease the vertical camera motion or not.
    //public bool forwardFocus = false; //  Shift the focus to the front of the jumper to enable wide forward view.
    //public bool projectedFocus = false; // Shift the focus to the projeced position of the jumber, which means running faster moves the focal point farther.
    //public bool platformSnapping = false; // Halt the vertical camera motion when the jumper is jumping.
    //public float cameraEasingNormal_x = 0.1; // Smoothness of the horizontal camera motion in normal state.
    //public float cameraEasingNormal_y = 0.1; // Smoothness of the vertical camera motion in normal state.
    //public float cameraEasingGrounding_y = 0.2; // Smoothness of the camera motion when the jumper grounded.
    //public float cameraWindow_h = 0; // Height of the camera window.
    //public float cameraWindow_w = 0; // Width of the camera window.
    //public float focusDistance = 100; // Distance to the focal point.
    //public float focusingSpeed = 5; // Velocity of the focal point movement.
    [HideInInspector] public float bgScrollRatio = 0.5f; // Determine the scroll speed of the BG layer.

    // Misc. parameters
    [PersistentAmongPlayMode] public bool showVelocityChart = false; // Show velocity chart in the chart canvas.
    [PersistentAmongPlayMode] public bool showTrail = false;// Show the trail or not.
    [PersistentAmongPlayMode] public bool showAfterimage = false; // Show afterimage instead of red dots when 'showTrail' is true.
    [PersistentAmongPlayMode] public bool showInputStatus = false; // Show the input status.

    public enum CharaType
    {
        oneHead,
        threeHead,
        eightHead,
        Toufu
    }
    [SerializeField] public CharaType charaType = CharaType.oneHead;
    private CharaType changeChara;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

    }
    private void Start()
    {
        //Application.targetFrameRate = 60; //FPSを60に設定 

        Masao = GameManager.Instance.Masao;
        Masaorb2D = Masao.GetComponent<Rigidbody2D>();
        jumper = Masao.GetComponent<Jumper>();
        Masao.GetComponent<echoEffect>().enabled = showTrail;
        changeChara = charaType;
        if (charaType == CharaType.oneHead)
        {
            Masao.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(GameManager.Instance.OneHeadAnimator);
        }
        else if (charaType == CharaType.threeHead)
        {
            Masao.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(GameManager.Instance.MasaoAnimator);
        }
        else if (charaType == CharaType.eightHead)
        {
            Masao.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(GameManager.Instance.RealAnimator);
        }
        else if (charaType == CharaType.Toufu)
        {
            Masao.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(GameManager.Instance.ToufuAnimator);
        }
    }

    private void Update()
    {
        if(showTrail != preShowTrail)
        {
            Masao.GetComponent<echoEffect>().enabled = showTrail;
            preShowTrail = showTrail;
        }

        if (charaType != changeChara)
        {
            if (charaType == CharaType.oneHead)
            {
                Masao.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(GameManager.Instance.OneHeadAnimator);
            }
            else if (charaType == CharaType.threeHead)
            {
                Masao.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(GameManager.Instance.MasaoAnimator);
            }
            else if (charaType == CharaType.eightHead)
            {
                Masao.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(GameManager.Instance.RealAnimator);
            }
            else if (charaType == CharaType.Toufu)
            {
                Masao.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(GameManager.Instance.ToufuAnimator);
            }
            changeChara = charaType;
        }
    }
}