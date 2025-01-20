using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBasedProjectile : Projectile
{
    private int numberOfLightSources = 0;
    [SerializeField] SpriteRenderer spriteRenderer = null;

    private void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void FixedUpdate()
    {
        if (numberOfLightSources > 0)
        {
            spriteRenderer.color = Color.red;
            dashable = true;
        } else
        {
            spriteRenderer.color = Color.black;
            dashable = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HopeLight")
        {
            numberOfLightSources++;
        }
    }

      private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "HopeLight")
        {
            numberOfLightSources--;
        }
    }
}
