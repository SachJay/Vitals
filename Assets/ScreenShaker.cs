using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    public AnimationCurve curve;
    public float duration = 0.3f;
    public float strength = 1f;

    public void ShakeScreen()
    {
        StartCoroutine(DoShakeScreen());
    }

    IEnumerator DoShakeScreen()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration) 
        {
            elapsedTime += Time.deltaTime;
            float curveStrength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * curveStrength * strength;

            yield return null;
        }

        transform.position = startPosition;
    }
}
