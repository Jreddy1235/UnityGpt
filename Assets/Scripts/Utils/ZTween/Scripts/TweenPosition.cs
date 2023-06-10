using UnityEngine;

/// <summary>
/// Tween GameObject's position.
/// </summary>
[AddComponentMenu("ZTween/Tween Position")]
public class TweenPosition : TweenBase
{
    public Vector3 from;
    public Vector3 to;

    [HideInInspector]
    public bool worldSpace = false;

    public Vector3 value
    {
        get
        {
            return worldSpace ? transform.position : transform.localPosition;
        }
        set
        {
            if (worldSpace)
            {
                transform.position = value;
            }
            else
            {
                transform.localPosition = value;
            }
        }
    }

    /// <summary>
    /// Call Tweening Logic.
    /// </summary>
    /// <param name="factor">Factor.</param>
    /// <param name="isFinished">If set to <c>true</c> is finished.</param>
    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = from * (1f - factor) + to * factor;
    }

    /// <summary>
    /// Begin tweening operation.
    /// </summary>
    /// <param name="pos">Final position value</param>
    static public TweenPosition Begin(GameObject go, float duration, Vector3 pos)
    {
        TweenPosition comp = TweenBase.Begin<TweenPosition>(go, duration);
        comp.from = comp.value;
        comp.to = pos;

        if (duration <= 0f)
        {
            comp.OnFactorModified(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    /// <summary>
    /// Begin tweening operation.
    /// </summary>
    /// <param name="pos">Final position value</param>
    /// <param name="worldSpace">If true, tween will take global values for position</param>
    static public TweenPosition Begin(GameObject go, float duration, Vector3 pos, bool worldSpace)
    {
        TweenPosition comp = TweenBase.Begin<TweenPosition>(go, duration);
        comp.worldSpace = worldSpace;
        comp.from = comp.value;
        comp.to = pos;

        if (duration <= 0f)
        {
            comp.OnFactorModified(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    /// <summary>
    /// Set 'from' value to current value.
    /// </summary>
    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue()
    {
        from = value;
    }

    /// <summary>
    /// Set 'to' value to the current value.
    /// </summary>
    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue()
    {
        to = value;
    }

    /// <summary>
    /// Sets current value to 'from' value.
    /// </summary>
    [ContextMenu("Assume value of 'From'")]
    public override void SetCurrentValueToStart()
    {
        value = from;
    }

    /// <summary>
    /// Sets current value to 'to' value.
    /// </summary>
    [ContextMenu("Assume value of 'To'")]
    public override void SetCurrentValueToEnd()
    {
        value = to;
    }
    [ContextMenu("Set 'Y' at 20 units apart")]
    public void SetYAt20UnitsApart()
    {
        int dir = transform.GetSiblingIndex() % 2 == 0 ? 1 : -1;
        from = value + 10 * Vector3.up * dir;
        to = value - 10 * Vector3.up * dir;
    }
}
