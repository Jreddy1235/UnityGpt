using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for all Tween operations.
/// </summary>
public abstract class TweenBase : MonoBehaviour
{

    public enum Direction
    {
        Reverse = -1,
        Toggle = 0,
        Forward = 1,
    }

    public enum Style
    {
        Once,
        Loop,
        PingPong,
        Repeat
    }

    /// <summary>
    /// Once, Loop or Ping Pong
    /// </summary>
    [HideInInspector]
    public Style style = Style.Once;

    /// <summary>
    /// Selected easing curve, linked to animation curve
    /// </summary>
    [HideInInspector]
    public int selectedEasing = 0;

    /// <summary>
    /// Evaluate factor based on animation curve
    /// </summary>
    [HideInInspector]
    public AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

    /// <summary>
    /// If set to true, timescale will be ignored
    /// </summary>
    [HideInInspector]
    public bool ignoreTimeScale = true;

    /// <summary>
    /// Delay before tweening operation starts
    /// </summary>
    [HideInInspector]
    public float delay = 0f;

    /// <summary>
    /// Duration of Tween
    /// </summary>
    [HideInInspector]
    public float duration = 1f;

    /// <summary>
    /// Events called once animation completes.
    /// </summary>
    [HideInInspector] public UnityEvent onFinished = new UnityEvent();
    [HideInInspector] public UnityEvent onFinishedOneShot = new UnityEvent();

    bool isStarted = false;
    float startTime = 0f;
    float mDuration = 0f;
    float mAmountPerDelta = 1000f;
    float factor = 0f;

    public float amountPerDelta
    {
        get
        {
            if (mDuration != duration)
            {
                mDuration = duration;
                mAmountPerDelta = Mathf.Abs((duration > 0f) ? 1f / duration : 1000f) * Mathf.Sign(mAmountPerDelta);
            }
            return mAmountPerDelta;
        }
    }

    /// <summary>
    /// Current Tween Progress : 0 to 1
    /// </summary>
    public float tweenFactor { get { return factor; } set { factor = Mathf.Clamp01(value); } }

    /// <summary>
    /// Direction of Tween
    /// </summary>
    public Direction direction { get { return amountPerDelta < 0f ? Direction.Reverse : Direction.Forward; } }

    void Reset()
    {
        if (!isStarted)
        {
            selectedEasing = 0;
            SetStartToCurrentValue();
            SetEndToCurrentValue();
        }
    }

    protected virtual void Start()
    {
        Update();
    }

    void Update()
    {
        float delta = ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
        float time = ignoreTimeScale ? Time.unscaledTime : Time.time;

        if (!isStarted)
        {
            isStarted = true;
            startTime = time + delay;
        }

        if (time < startTime)
            return;

        factor += amountPerDelta * delta;

        if (style == Style.Loop)
        {
            if (factor > 1f)
            {
                factor -= Mathf.Floor(factor);
            }
            else if (factor < 0f)
            {
                factor = -factor;
                mAmountPerDelta = Mathf.Abs(mAmountPerDelta);
            }
        }
        else if (style == Style.PingPong)
        {
            if (factor > 1f)
            {
                factor = 1f - (factor - Mathf.Floor(factor));
                mAmountPerDelta = -mAmountPerDelta;
            }
            else if (factor < 0f)
            {
                factor = -factor;
                factor -= Mathf.Floor(factor);
                mAmountPerDelta = -mAmountPerDelta;
            }
        }

        if ((style == Style.Once) && (duration == 0f || factor > 1f || factor < 0f))
        {
            factor = Mathf.Clamp01(factor);
            OnFactorModified(factor, true);
            enabled = false;
            if (onFinishedOneShot != null)
            {
                onFinishedOneShot.Invoke();
                onFinishedOneShot = new UnityEvent();
            }
            if (onFinished != null)
            {
                onFinished.Invoke();
            }
        }
        else
            OnFactorModified(factor, false);
    }

    /// <summary>
    /// Sets event to execute once tweening operation completes
    /// </summary>
    /// <param name="oneShot">if true, event will be called only once</param>
    public void SetOnFinished(UnityAction del, bool oneShot = false)
    {
        if (oneShot)
        {
            onFinishedOneShot = new UnityEvent();
            onFinishedOneShot.AddListener(del);
        }
        else
        {
            onFinished = new UnityEvent();
            onFinished.AddListener(del);
        }
    }

    /// <summary>
    /// Add event to execute once tweening operation completes
    /// </summary>
    /// <param name="oneShot">if true, event will be called only once</param>
    public void AddOnFinished(UnityAction del, bool oneShot = false)
    {
        if (oneShot)
            onFinishedOneShot.AddListener(del);
        else
            onFinished.AddListener(del);
    }

    /// <summary>
    /// Remove event to execute once tweening operation completes
    /// </summary>
    public void RemoveOnFinished(UnityAction del)
    {
        onFinished.RemoveListener(del);
    }

    /// <summary>
    /// Remove all events.
    /// </summary>
    public void RemoveAllOnFinished()
    {
        onFinished = new UnityEvent();
        onFinishedOneShot = new UnityEvent();
    }

    private void OnEnable()
    {
        if (style == Style.Repeat)
        {
            if (direction == Direction.Forward)
            {
                SetCurrentValueToStart();
                Play(true);
            }
            else
            {
                SetCurrentValueToEnd();
                Play(false);
            }

        }
    }

    private void OnDisable()
    {
        isStarted = false;
    }

    /// <summary>
    /// Calls tween logic on factor increment/decrement
    /// </summary>
    public void OnFactorModified(float factor, bool isFinished)
    {
        float val = Mathf.Clamp01(factor);
        OnUpdate((animationCurve != null) ? animationCurve.Evaluate(val) : val, isFinished);
    }

    /// <summary>
    /// Play tweening operation in forward direction.
    /// </summary>
    public void PlayForward()
    {
        Play(true);
    }

    /// <summary>
    /// Play tweening operation in reverse direction.
    /// </summary>
    public void PlayReverse()
    {
        Play(false);
    }

    /// <summary>
    /// Start the tweening operation by passing direction manually
    /// </summary>
    public void Play(bool forward)
    {
        isStarted = false;
        factor = forward ? 0 : 1;
        mAmountPerDelta = Mathf.Abs(amountPerDelta);
        if (!forward)
            mAmountPerDelta = -mAmountPerDelta;
        enabled = true;
        Update();
    }

    /// <summary>
    /// Toggle tweening direction
    /// </summary>
    public void Toggle()
    {
        if (factor > 0f)
        {
            mAmountPerDelta = -amountPerDelta;
        }
        else
        {
            mAmountPerDelta = Mathf.Abs(amountPerDelta);
        }
        enabled = true;
    }

    /// <summary>
    /// Call Tweening Logic.
    /// </summary>
    /// <param name="factor">Factor.</param>
    /// <param name="isFinished">If set to <c>true</c> is finished.</param>
    abstract protected void OnUpdate(float factor, bool isFinished);

    /// <summary>
    /// Begin tweening operation.
    /// </summary>
    static public T Begin<T>(GameObject go, float duration) where T : TweenBase
    {
        T comp = go.GetComponent<T>();
        if (comp == null)
        {
            comp = go.AddComponent<T>();

            comp.animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
            comp.style = Style.Once;
        }
        comp.isStarted = false;
        comp.duration = duration;
        comp.factor = 0f;
        comp.mAmountPerDelta = Mathf.Abs(comp.amountPerDelta);
        comp.enabled = true;
        return comp;
    }

    /// <summary>
    /// Set 'from' value to current value.
    /// </summary>
    public virtual void SetStartToCurrentValue()
    {
    }

    /// <summary>
    /// Set 'to' value to the current value.
    /// </summary>
    public virtual void SetEndToCurrentValue()
    {
    }

    /// <summary>
    /// Sets current value to 'from' value.
    /// </summary>
    public virtual void SetCurrentValueToStart()
    {
    }

    /// <summary>
    /// Sets current value to 'to' value.
    /// </summary>
    public virtual void SetCurrentValueToEnd()
    {
    }
}
