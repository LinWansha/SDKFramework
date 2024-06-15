using UnityEngine;

public static class ColorExt
{
    public static string ToHexColor(this Color color)
    {
        if (color == Color.white)
            return "#000000";

        int r = Mathf.Clamp((int)(color.r * 255), 0, 255);
        int g = Mathf.Clamp((int)(color.g * 255), 0, 255);
        int b = Mathf.Clamp((int)(color.b * 255), 0, 255);

        string R = r.ToString("X2");
        string G = g.ToString("X2");
        string B = b.ToString("X2");

        return $"#{R}{G}{B}".ToUpper();
    }
}

public static class StringExt
{
    public static string DoMagic(this string tag, Color color)
    {
        string hexColor = color.ToHexColor();
        return $"<color={hexColor}>{tag}</color>";
    }
} 