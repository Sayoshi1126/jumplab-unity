using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigidbody2d;
    float vec;
    float speed = 10f;
    private GameObject player;

    float rate;
    float localscale;

    void Start()
    {
        rate = 0;
        GameManager.Instance.bulletNum += 1;
        vec = Settings.Instance.shotAngle;
        speed = Settings.Instance.bulletSpeed.Evaluate(rate)*Settings.Instance.bulletMaxSpeed;
        float destroyTime = Settings.Instance.destroyTime;

        player = GameObject.FindWithTag("Player");
        localscale = player.transform.localScale.x;
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = new Vector2(speed*player.transform.localScale.x*Mathf.Cos(vec / 180 * Mathf.PI) ,speed*Mathf.Sin(vec / 180 * Mathf.PI));

        Invoke("destroyMethod", Settings.Instance.destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        rate += Time.deltaTime;
        speed = Settings.Instance.bulletSpeed.Evaluate(rate / Settings.Instance.destroyTime) * Settings.Instance.bulletMaxSpeed;
        rigidbody2d.velocity = new Vector2(speed * localscale * Mathf.Cos(vec / 180 * Mathf.PI), speed * Mathf.Sin(vec / 180 * Mathf.PI));
        //rigidbody2d.velocity = transform.right * speed * player.transform.localScale.x;
    }

    void destroyMethod()
    {
        GameManager.Instance.bulletNum -= 1;
        Destroy(gameObject);
    }
}
