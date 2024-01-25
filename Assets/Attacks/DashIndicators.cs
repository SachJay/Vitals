using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashIndicators : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer render;

    [SerializeField]
    Player player;

    [SerializeField]
    int minAmount = 2;

    [SerializeField]
    bool isDash = true;

    public void StartAttackTimer()
    {

    } 

    public void StartDashTimer()
    {
        
    }

    void Update()
    {
        //if (isDash)
        //{
        //    render.enabled = player.currentDashCount >= minAmount;
        //} else
        //{
        //    render.enabled = player.currentAttackCount >= minAmount;
        //}

    }
}
