using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public static class Startup
{
    public static Rect Add(this Rect rect, float x, float y, float width, float height)
    {
        return new Rect(rect.x + x, rect.y + y, rect.width + width, rect.height + height);
    }
}