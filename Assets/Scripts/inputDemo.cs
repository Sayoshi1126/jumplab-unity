using System.Collections.Generic;
using UnityEngine;
using InControl;
public class inputDemo : MonoBehaviour
{

    // Update is called once per frame
    private void Start()
    {
        
    }
    void Update()
    {
        var ad = InputManager.ActiveDevice;

        // ボタンが押されたかを表示
        if (ad.DPadLeft.WasPressed) Debug.Log("方向キー左が押された");
        if (ad.DPadRight.WasPressed) Debug.Log("方向キー右が押された");
        if (ad.DPadUp.WasPressed) Debug.Log("方向キー上が押された");
        if (ad.DPadDown.WasPressed) Debug.Log("方向キー下が押された");
        if (ad.Action1.WasPressed) Debug.Log("×ボタンが押された");
        if (ad.Action2.WasPressed) Debug.Log("○ボタンが押された");
        if (ad.Action3.WasPressed) Debug.Log("□ボタンが押された");
        if (ad.Action4.WasPressed) Debug.Log("△ボタンが押された");
        if (ad.LeftBumper.WasPressed) Debug.Log("L1ボタンが押された");
        if (ad.RightBumper.WasPressed) Debug.Log("R1ボタンが押された");
        if (ad.LeftTrigger.WasPressed) Debug.Log("L2ボタンが押された");
        if (ad.RightTrigger.WasPressed) Debug.Log("R2ボタンが押された");
        if (ad.LeftStickButton.WasPressed) Debug.Log("L3ボタンが押された");
        if (ad.RightStickButton.WasPressed) Debug.Log("R3 ボタンが押された");

        if (ad.GetControl(InputControlType.Options).WasPressed) Debug.Log("OPTIONSボタンが押された");
        if (ad.GetControl(InputControlType.Share).WasPressed) Debug.Log("SHAREボタンが押された");
        if (ad.GetControl(InputControlType.System).WasPressed) Debug.Log("PSボタンが押された");
        if (ad.GetControl(InputControlType.TouchPadButton).WasPressed) Debug.Log("パッドボタンが押された");


        // 左スティックの傾きを表示
        Debug.Log("左スティック横方向の傾き: " + ad.LeftStick.X); // -1～1
        Debug.Log("左スティック縦方向の傾き: " + ad.LeftStick.Y); // -1～1
                                                      // Debug.Log("左スティック左方向の傾き: " + ad.LeftStickLeft.Value); // 0～1
                                                      // Debug.Log("左スティック右方向の傾き: " + ad.LeftStickRight.Value); // 0～1
                                                      // Debug.Log("左スティック上方向の傾き: " + ad.LeftStickUp.Value); // 0～1
                                                      // Debug.Log("左スティック下方向の傾き: " + ad.LeftStickDown.Value); // 0～1

        // 右スティックの傾きを表示
        Debug.Log("右スティック横方向の傾き: " + ad.RightStick.X); // -1～1
        Debug.Log("右スティック縦方向の傾き: " + ad.RightStick.Y); // -1～1
        // Debug.Log("右スティック左方向の傾き: " + ad.RightStickLeft.Value); // 0～1
        // Debug.Log("右スティック右方向の傾き: " + ad.RightStickRight.Value); // 0～1
        // Debug.Log("右スティック上方向の傾き: " + ad.RightStickUp.Value); // 0～1
        // Debug.Log("右スティック下方向の傾き: " + ad.RightStickDown.Value); // 0～1
    }
}