using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using InControl;

public class ControllerManager : SingletonMonoBehaviour<ControllerManager>
{
    // Start is called before the first frame update

    public enum func
    {
        None,
        Action1,
        Action2,
        Action3,
        Action4,
        LeftBumper,
        RightBumper,
        LeftTrigger,
        RightTrigger,
        LeftStickButton,
        RightStickButton,
    }

    public struct Button
    {
        public bool WasPressed;
        public bool IsPressed;
        public bool WasReleased;
    }

    public struct Stick
    {
        public Vector2 PadVector;
    }

    [SerializeField] private func Jump;
    public Button jump;
    [SerializeField] private func Bullet;
    public Button bullet;
    [SerializeField] private func Cut;
    public Button cut;
    [SerializeField] private func Menu;
    public Button menu;
    void Start()
    {

    }
    private void Update()
    {
        jump = setButtonPara(jump,Jump);
        //bullet = setButtonPara(bullet, Bullet);
        //cut = setButtonPara (cut, Cut);
        menu = setButtonPara(menu,Menu);
    }

    Button setButtonPara(Button inputButton, func function)
    {
        var ad = InputManager.ActiveDevice;
        if (function == func.Action1)
        {
            inputButton.WasPressed = ad.Action1.WasPressed;
            inputButton.IsPressed = ad.Action1.IsPressed;
            inputButton.WasReleased = ad.Action1.WasReleased;
        }
        else if (function == func.Action2)
        {
            inputButton.WasPressed = ad.Action2.WasPressed;
            inputButton.IsPressed = ad.Action2.IsPressed;
            inputButton.WasReleased = ad.Action2.WasReleased;
        }
        else if (function == func.Action3)
        {
            inputButton.WasPressed = ad.Action3.WasPressed;
            inputButton.IsPressed = ad.Action3.IsPressed;
            inputButton.WasReleased = ad.Action3.WasReleased;
        }
        else if (function == func.Action4)
        {
            inputButton.WasPressed = ad.Action4.WasPressed;
            inputButton.IsPressed = ad.Action4.IsPressed;
            inputButton.WasReleased = ad.Action4.WasReleased;
        }
        else if (function == func.LeftBumper)
        {
            inputButton.WasPressed = ad.LeftBumper.WasPressed;
            inputButton.IsPressed = ad.LeftBumper.IsPressed;
            inputButton.WasReleased = ad.LeftBumper.WasReleased;
        }
        else if (function == func.RightBumper)
        {
            inputButton.WasPressed = ad.RightBumper.WasPressed;
            inputButton.IsPressed = ad.RightBumper.IsPressed;
            inputButton.WasReleased = ad.RightBumper.WasReleased;

        }
        else if (function == func.LeftTrigger)
        {
            inputButton.WasPressed = ad.LeftTrigger.WasPressed;
            inputButton.IsPressed = ad.LeftTrigger.IsPressed;
            inputButton.WasReleased = ad.LeftTrigger.WasReleased;

        }
        else if (function == func.RightTrigger)
        {
            inputButton.WasPressed = ad.RightTrigger.WasPressed;
            inputButton.IsPressed = ad.RightTrigger.IsPressed;
            inputButton.WasReleased = ad.RightTrigger.WasReleased;

        }
        else if (function == func.LeftStickButton)
        {
            inputButton.WasPressed = ad.LeftStickButton.WasPressed;
            inputButton.IsPressed = ad.LeftStickButton.IsPressed;
            inputButton.WasReleased = ad.LeftStickButton.WasReleased;
        }
        else if (function == func.RightStickButton)
        {
            inputButton.WasPressed = ad.RightStickButton.WasPressed;
            inputButton.IsPressed = ad.RightStickButton.IsPressed;
            inputButton.WasReleased = ad.RightStickButton.WasReleased;

        }
        return inputButton;
    }
}
