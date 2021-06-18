using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    int x, h;
    int cw, ch;
    int bw = 48;
    int bh = 48;
    int sx, sy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //int getChip(int x, int y)
    //{
    //    int cx = x / bw;
    //    int cy = y / bh;
    //    if (cx < 0 || cy < 0 || cx >= cw || cy >= ch) return 0;
    //    return map[cx][cy];
    //}
    //bool isThereObstacle(int x, int y)
    //{
    //    return getChip(x,y)>0;
    //}

    float obstaclePenaltyL(float x)
    {
        int cx = (int)(Math.Ceiling(x / bw));
        return cx * bw - x;
    }

    float obstaclePenaltyR(float x)
    {
        int cx = (int)(Math.Ceiling(x / bw));
        return x-cx*bw;
    }
    float obstaclePenaltyU(float y)
    {
        int cy = (int)(Math.Ceiling(y / bh));
        return cy * bw - y;
    }
    float obstaclePenaltyD(float y)
    {
        int cy = (int)(Math.Ceiling(y / bh));
        return y - cy * bw;
    }
}
