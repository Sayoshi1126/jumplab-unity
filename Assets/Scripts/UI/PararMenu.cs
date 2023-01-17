using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PararMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Slider maxVX;
    [SerializeField] Slider maxVY;

    [SerializeField] Slider jumpPower;
    [SerializeField] Slider gravityRising;
    [SerializeField] Slider gravityFalling;
    [SerializeField] Slider verticalSpeedSustainLevel;

    [SerializeField] Slider axNormal;
    [SerializeField] Slider axBrake;
    [SerializeField] Slider axJumping;

    [SerializeField] Slider OrthoSize;
    [SerializeField] Slider xDamping;
    [SerializeField] Slider yDamping;
    [SerializeField] Slider focusDistance;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer transposer;
    private CinemachineVirturalCameraCus cus;

    void Start()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        cus = virtualCamera.GetComponent<CinemachineVirturalCameraCus>();

        maxVX.value = 0.2f;
        maxVY.value = 0.2f;
        axNormal.value = 1f;
        axBrake.value = 0.05f;
        axJumping.value = 0f;
        jumpPower.value = 0.6f;
        gravityRising.value = 1f;
        gravityFalling.value = 1f;
        verticalSpeedSustainLevel.value = 0f;
        OrthoSize.value = 0.5f;
        xDamping.value = 0f;
        yDamping.value = 0f;
        focusDistance.value = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Settings.Instance.jumpParam.maxVx = maxVX.value * 10;
        Settings.Instance.jumpParam.maxVy = maxVY.value * 15;
        Settings.Instance.jumpParam.jumpVelocity = jumpPower.value*700;
        Settings.Instance.jumpParam.axNormal = axNormal.value*0.5f;
        Settings.Instance.jumpParam.axBrake = axBrake.value * 0.5f;
        Settings.Instance.jumpParam.axJumping = axJumping.value * 0.5f;
        Settings.Instance.jumpParam.gravityRising = gravityRising.value * 6;
        Settings.Instance.jumpParam.gravityFalling = gravityFalling.value * 6;
        Settings.Instance.jumpParam.verticalSpeedSustainLevel = 1-verticalSpeedSustainLevel.value;

        virtualCamera.m_Lens.OrthographicSize = OrthoSize.value * 5;
        transposer.m_XDamping = xDamping.value * 5;
        transposer.m_YDamping = yDamping.value * 5;
        cus.focusDistance = focusDistance.value * 4;
    }

    public void OneButton()
    {
        Settings.Instance.charaType = Settings.CharaType.oneHead;
    }
    public void ThreeButton()
    {
        Settings.Instance.charaType = Settings.CharaType.threeHead;
    }
    public void EightButton()
    {
        Settings.Instance.charaType = Settings.CharaType.eightHead;
    }
}
