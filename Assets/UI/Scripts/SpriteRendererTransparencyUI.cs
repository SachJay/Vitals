using UnityEngine;

public class SpriteRendererTransparencyUI : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color currentColor;

    private void Awake()
    {
        if (!TryGetComponent(out spriteRenderer))
            LogExtension.LogMissingComponent(name, nameof(SpriteRenderer));
    }

    private void Start()
    {
        currentColor = spriteRenderer.color;
    }

    public void SetTransparency(float transparencyPercentage)
    {
        spriteRenderer.color = new(currentColor.r, currentColor.g, currentColor.b, transparencyPercentage);
    }
}
