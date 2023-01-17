using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ChangeMenu : MonoBehaviour
{

    GameObject parent;
    Transform[] children;

    GameObject preMenu;
    GameObject nowMenu;
    private void Start()
    {
        preMenu = this.gameObject;
        nowMenu = this.gameObject;
    }

    public void Update()
    {
        parent = this.gameObject;

        children = new Transform[parent.transform.childCount];

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            children[i] = parent.transform.GetChild(i);
            if (children[i].gameObject.activeSelf&children[i].gameObject!=nowMenu)
            {
                preMenu = nowMenu;
                nowMenu = children[i].gameObject;
            }
        }
    }
    public void OnClick()
    {
        Debug.Log(preMenu.gameObject.name);
        Debug.Log(nowMenu.gameObject.name);
        preMenu.SetActive(true);
        nowMenu.SetActive(false);
    }
}
