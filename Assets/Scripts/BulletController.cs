using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigidbody2d;
    float speed = 10f;
    private GameObject player;

    void Start()
    {
        speed = Settings.Instance.bulletSpeed;
        float destroyTime = Settings.Instance.destroyTime;

        player = GameObject.FindWithTag("Player");
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = transform.right * speed * player.transform.localScale.x;

        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
