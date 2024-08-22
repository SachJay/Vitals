using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    enum ColorPalate {
        PRIMARY,
        SECONDARY,
        DARK,
        LIGHT_DARK,
        DANGER,
        WARNING
    }

    string primary = "#FFFFFF";
    string secondary = "#AC2432";
    string dark = "#1C032B";
    string lightDark = "#AC2432";
    string danger = "#4F094C";
    string warning = "#E29453";

    [SerializeField]
    ColorPalate overrideColor;

    [SerializeField]
    SpriteRenderer[] spriteRenderers;

    [SerializeField]
    Image[] images;

    // Start is called before the first frame update
    void Start()
    {
        updateColor(getColor());
    }

    private string getColor()
    {
        switch (overrideColor)
        {
            case ColorPalate.PRIMARY:
                return primary;

            case ColorPalate.SECONDARY:
                return secondary;

            case ColorPalate.DARK:
                return dark;

            case ColorPalate.LIGHT_DARK:
                return lightDark;

            case ColorPalate.WARNING:
                return warning;

            case ColorPalate.DANGER:
                return danger;

            default:
                return primary;
        }
    }

    private void updateColor(string hexColor)
    {
        Color colorFromHex;
        ColorUtility.TryParseHtmlString(hexColor, out colorFromHex);

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = colorFromHex;
        }

        foreach (Image image in images)
        {
            image.color = colorFromHex;
        }
    }
}
