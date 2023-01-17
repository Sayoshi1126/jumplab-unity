using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordHitbox : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Hitbox;
    [SerializeField] Jumper jumper;
    void attackHitboxStart()
    {
        Hitbox.gameObject.SetActive(true);
    }
    void attackHitboxEnd()
    {
        Hitbox.gameObject.SetActive(false);
    }

    void atackEnd()
    {
        jumper.canControl = true;
    }
}
