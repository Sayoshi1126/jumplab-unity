using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject paraMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if(ControllerManager.Instance.menu.WasPressed)
        {
            if (paraMenu.activeSelf == false)
            {
                SelectParaMenu();
            }
            else
            {
                CloseParaMenu();
            }
        }
    }

    public void SelectParaMenu()
    {
        paraMenu.SetActive(true);
    }

    public void CloseParaMenu()
    {
        paraMenu.SetActive(false);
    }
}
