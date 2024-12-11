using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSquisher : MonoBehaviour
{

    [SerializeField] private float minSquishSpeed = 0;
    [SerializeField] private float maxSquishSpeed = 20;
    [SerializeField] private float minSquishPercent = 1;
    [SerializeField] private float maxSquishPercent = 0.75f;
    [SerializeField] private Player player;
    private float squishValue;

    private Vector3 originalScale;
    
    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        if (player.PlayerAttack.IsAttacking || player.PlayerDash.IsDashing)
        {
            squishValue = scale(maxSquishSpeed);
        }
        else
        {
            squishValue = scale(player.PlayerMovement.GetVelocity().magnitude);
        }

        transform.localScale = new Vector3(originalScale.x, originalScale.y * squishValue, originalScale.z);
    }

    public float scale(float currentValue)
    {

        float OldRange = (maxSquishSpeed - minSquishSpeed);
        float NewRange = (maxSquishPercent - minSquishPercent);
        float NewValue = (((currentValue - minSquishSpeed) * NewRange) / OldRange) + minSquishPercent;

        return (NewValue);
    }
}
