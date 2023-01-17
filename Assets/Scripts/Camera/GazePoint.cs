using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazePoint : MonoBehaviour
{
    // Start is called before the first frame update
    public bool platFormSnaping;
    Rigidbody2D rigidbody2d;
    private GameObject jumper;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        jumper = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(platFormSnaping)
        {
            transform.position = new Vector2(jumper.transform.position.x, transform.position.y);
        }
        else
        {
            transform.position = jumper.transform.position;
        }
    }
}
