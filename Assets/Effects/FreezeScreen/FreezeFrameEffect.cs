using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeFrameEffect : MonoBehaviour
{
    [SerializeField]
    float duration = 0.2f;

    bool isScreenFreezed = false;

    public IEnumerator FreezeGame()
    {
        if (!isScreenFreezed)
        {
            yield return doFreezeGame();
        }
    }

    private IEnumerator doFreezeGame()
    {
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        isScreenFreezed = true;

        yield return new WaitForSecondsRealtime(duration);

        isScreenFreezed = false;
        Time.timeScale = originalTimeScale;
    }
}
