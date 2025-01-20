using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpell : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    private void Start()
    {
        Destroy(this, duration);
    }
}
