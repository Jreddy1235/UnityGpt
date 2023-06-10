using UnityEngine;

/// <summary>
/// Hops the object's position.
/// </summary>
using System.Collections;

public class TweenHop : TweenBase
{
    public Vector3 from;
    public Vector3 to;
    [Range(-1, 1)]
    public float curvature = .72f;
    public float offsetAngle;

    [HideInInspector]
    public bool
        worldSpace = false;

    float hopHeight;

    public float HopHeight
    {
        get
        {
            hopHeight = (value.x - to.x) * curvature;
            return hopHeight;
        }
    }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public Vector3 value
    {
        get
        {
            return worldSpace ? transform.position : transform.localPosition;
        }
        set
        {
            Vector3 dir = default(Vector3);
            float sign = 0;
            if (worldSpace)
            {
                dir = value - transform.position;
                sign = Mathf.Sign(value.x - transform.position.x);
                transform.position = value;
            }
            else
            {
                dir = value - transform.localPosition;
                sign = Mathf.Sign(value.x - transform.localPosition.x);
                transform.localPosition = value;
            }
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var scale = transform.localScale;
            scale.x = sign * Mathf.Abs(scale.x);
            transform.localScale = scale;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward) *
                Quaternion.Euler(0, 0, sign > 0 ? offsetAngle : -(offsetAngle - 180));
        }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        var height = Mathf.Sin(Mathf.PI * factor) * HopHeight;
        value = Vector3.Lerp(from, to, factor) + Vector3.up * height;
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenHop Begin(GameObject go, float duration, Vector3 pos)
    {
        TweenHop comp = TweenBase.Begin<TweenHop>(go, duration);
        comp.from = comp.value;
        comp.to = pos;

        if (duration <= 0f)
        {
            comp.OnFactorModified(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue()
    {
        from = value;
    }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue()
    {
        to = value;
    }

    [ContextMenu("Assume value of 'From'")]
    public override void SetCurrentValueToStart()
    {
        value = from;
    }

    [ContextMenu("Assume value of 'To'")]
    public override void SetCurrentValueToEnd()
    {
        value = to;
    }
}
