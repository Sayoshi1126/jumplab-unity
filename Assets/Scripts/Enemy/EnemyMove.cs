using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Vector2 Turn(bool edgeTurn, bool wallTurn, Vector2 pos, Vector2 localScale)
    {
        float offSet = 0.1f;
        Ray2D rayFront = new Ray2D((Vector2)pos + Vector2.up * 0.32f + Vector2.right * 0.16f * localScale.x, Vector2.right * localScale.x);
        Ray2D rayRight = new Ray2D((Vector2)pos + offSet * Vector2.right, Vector2.up * -1);
        Ray2D rayLeft = new Ray2D((Vector2)pos + offSet * Vector2.left, Vector2.up * -1);

        RaycastHit2D hitCastFront = Physics2D.Raycast(rayFront.origin, rayFront.direction, offSet);
        RaycastHit2D hitCastRight = Physics2D.Raycast(rayRight.origin, rayRight.direction, offSet * 2);
        RaycastHit2D hitCastLeft = Physics2D.Raycast(rayLeft.origin, rayLeft.direction, offSet * 2);

        Debug.DrawRay(rayFront.origin, offSet * rayFront.direction, Color.yellow);
        Debug.DrawRay(rayRight.origin, (offSet * 2) * rayRight.direction, Color.red);
        Debug.DrawRay(rayLeft.origin, (offSet * 2) * rayLeft.direction, Color.blue);

        if (edgeTurn)
        {
            if (hitCastLeft.distance == 0 && hitCastRight.distance != 0 && hitCastRight.collider.tag == "ground")
            {
                localScale = new Vector2(1, localScale.y);
            }
            else if (hitCastLeft.distance != 0 && hitCastRight.distance == 0 && hitCastLeft.collider.tag == "ground")
            {
                localScale = new Vector2(-1, localScale.y);
            }
        }

        if (wallTurn)
        {
            if (hitCastFront.distance != 0 && hitCastFront.collider.tag == "ground")
            {
                localScale = new Vector2(localScale.x * -1, localScale.y);
            }
        }

        return transform.localScale;
    }


}
