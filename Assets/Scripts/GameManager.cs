﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [HideInInspector] public Rigidbody2D Masaorb2D;
    [HideInInspector] public Jumper jumper;
    public GameObject Masao;

    public int bulletNum;

    public RuntimeAnimatorController MasaoAnimator;
    public RuntimeAnimatorController OneHeadAnimator;
    public RuntimeAnimatorController RealAnimator;
    public RuntimeAnimatorController ToufuAnimator;

    public GameObject Menu;

    // Start is called before the first frame update
    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Masaorb2D = Masao.GetComponent<Rigidbody2D>();
        jumper = Masao.GetComponent<Jumper>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ControllerManager.Instance.menu.WasPressed)
        {
            Menu.gameObject.SetActive(!Menu.gameObject.activeSelf);
        }
    }
}
