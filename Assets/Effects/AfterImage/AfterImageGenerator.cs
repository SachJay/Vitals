using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageGenerator : MonoBehaviour
{
    [SerializeField] private float _afterImageDuration = 1f;
    [SerializeField] private float _afterImageFrequency = 0.1f;
    [SerializeField] private SpriteRenderer sprite;

    public void CallGenerateAfterImages()
    {
        StartCoroutine(GenerateAfterImages());
    }

    private IEnumerator GenerateAfterImages()
    {
        float elapsedTime = 0f;
        float previousAfterImageTime = 0f;

        while (elapsedTime < _afterImageDuration)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > previousAfterImageTime + _afterImageFrequency)
            {
                Instantiate(sprite, transform.position, Quaternion.identity);
                previousAfterImageTime = elapsedTime;
            }
            
            yield return null;
        }
    }
}
