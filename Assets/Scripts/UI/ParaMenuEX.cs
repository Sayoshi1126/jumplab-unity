using UnityEngine;
using UnityEngine.UI;

public class ParaMenuEX : MonoBehaviour
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

    [SerializeField] Toggle AerialInertia;

    int frame;

    void Start()
    {

        maxVX.value = Settings.Instance.jumpParam.maxVx / 10;
        maxVY.value = Settings.Instance.jumpParam.maxVy / 15;
        axNormal.value = Settings.Instance.jumpParam.axNormal / 2f;
        axBrake.value = Settings.Instance.jumpParam.axBrake / 2f;
        axJumping.value = Settings.Instance.jumpParam.axJumping / 2f;
        jumpPower.value = Settings.Instance.jumpParam.jumpVelocity / 700;
        gravityRising.value = Settings.Instance.jumpParam.gravityRising / 6;
        gravityFalling.value = Settings.Instance.jumpParam.gravityFalling / 6;
        verticalSpeedSustainLevel.value = 1 - Settings.Instance.jumpParam.verticalSpeedSustainLevel;
        AerialInertia.isOn = Settings.Instance.jumpParam.aerialInertia;
        frame = 0;
    }

    // Update is called once per frame

    private void Update()
    {
        maxVX.value = Settings.Instance.jumpParam.maxVx / 10;
        maxVY.value = Settings.Instance.jumpParam.maxVy / 15;
        axNormal.value = Settings.Instance.jumpParam.axNormal / 2f;
        axBrake.value = Settings.Instance.jumpParam.axBrake / 2f;
        axJumping.value = Settings.Instance.jumpParam.axJumping / 2f;
        jumpPower.value = Settings.Instance.jumpParam.jumpVelocity / 700;
        gravityRising.value = Settings.Instance.jumpParam.gravityRising / 6;
        gravityFalling.value = Settings.Instance.jumpParam.gravityFalling / 6;
        verticalSpeedSustainLevel.value = 1 - Settings.Instance.jumpParam.verticalSpeedSustainLevel;
        AerialInertia.isOn = Settings.Instance.jumpParam.aerialInertia;

        maxVX.onValueChanged.AddListener(delegate { Settings.Instance.jumpParam.maxVx = maxVX.value * 10; });
        maxVY.onValueChanged.AddListener(delegate { Settings.Instance.jumpParam.maxVy = maxVY.value * 15; });
        jumpPower.onValueChanged.AddListener(delegate { Settings.Instance.jumpParam.jumpVelocity = jumpPower.value * 700; });
        axNormal.onValueChanged.AddListener(delegate { Settings.Instance.jumpParam.axNormal = axNormal.value * 2f; });
        axBrake.onValueChanged.AddListener(delegate { Settings.Instance.jumpParam.axBrake = axBrake.value * 2f; });
        axJumping.onValueChanged.AddListener(delegate { Settings.Instance.jumpParam.axJumping = axJumping.value * 2f; });
        gravityRising.onValueChanged.AddListener(delegate { Settings.Instance.jumpParam.gravityRising = gravityRising.value * 6; });
        gravityFalling.onValueChanged.AddListener(delegate { Settings.Instance.jumpParam.gravityFalling = gravityFalling.value * 6; });
        verticalSpeedSustainLevel.onValueChanged.AddListener(delegate { Settings.Instance.jumpParam.verticalSpeedSustainLevel = 1 - verticalSpeedSustainLevel.value; });
        AerialInertia.onValueChanged.AddListener(delegate { Settings.Instance.jumpParam.aerialInertia = AerialInertia.isOn; });

        //Settings.Instance.jumpParam.maxVx = maxVX.value * 10;
        //Settings.Instance.jumpParam.maxVy = maxVY.value * 15;
        //Settings.Instance.jumpParam.jumpVelocity = jumpPower.value * 700;
        //Settings.Instance.jumpParam.axNormal = axNormal.value * 0.5f;
        //Settings.Instance.jumpParam.axBrake = axBrake.value * 0.5f;
        //Settings.Instance.jumpParam.axJumping = axJumping.value * 0.5f;
        //Settings.Instance.jumpParam.gravityRising = gravityRising.value * 6;
        //Settings.Instance.jumpParam.gravityFalling = gravityFalling.value * 6;
        //Settings.Instance.jumpParam.verticalSpeedSustainLevel = 1 - verticalSpeedSustainLevel.value;
    }
    void LateUpdate()
    {

    }
}
