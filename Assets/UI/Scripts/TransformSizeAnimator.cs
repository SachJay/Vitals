using System.Collections;
using UnityEngine;

public class TransformSizeAnimator : MonoBehaviour
{
    [SerializeField, Range(-100.0f, 100.0f)] private float growPercentage = 10.0f;
    [SerializeField] private float animationDuration = 0.1f;

    public void StartAnimation()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Vector3 initialSize = transform.localScale;
        Vector3 targetScale = initialSize * (1.0f + growPercentage / 100.0f);

        for (float time = 0.0f; time < animationDuration * 2; time += Time.deltaTime)
        {
            float progress = Mathf.PingPong(time, animationDuration) / animationDuration;
            transform.localScale = Vector3.Lerp(initialSize, targetScale, progress);
            yield return null;
        }

        transform.localScale = initialSize;
    }
}
