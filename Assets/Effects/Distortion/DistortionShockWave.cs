using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistortionShockWave : MonoBehaviour
{

    [SerializeField] private float _shockWaveTime = 0.75f;
    private Coroutine _shockWaveCoroutine;
    private float startPos = -0.1f;
    private float endPos = 1f;

    private Material _material;

    private static int _waveDistanceFromCenter = Shader.PropertyToID("_ShockWaveDistanceFromCenter");

    private void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    public void CallShockWave(Vector2 position)
    {
        _shockWaveCoroutine = StartCoroutine(ShockWaveAction(position));
    }

    private IEnumerator ShockWaveAction(Vector2 position)
    {
        float elapsedTime = 0f;
        float distance = startPos;
        transform.position = position;

        do
        {
            elapsedTime += Time.deltaTime;

            _material.SetFloat(_waveDistanceFromCenter, distance);
            distance = Mathf.Lerp(startPos, endPos, (elapsedTime / _shockWaveTime));

            yield return null;

        } while (elapsedTime < _shockWaveTime);
    }
}
