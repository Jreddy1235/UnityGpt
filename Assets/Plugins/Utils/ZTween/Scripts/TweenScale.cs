using UnityEngine;

/// <summary>
/// Tween GameObject's local scale.
/// </summary>
[AddComponentMenu ("ZTween/Tween Scale")]
public class TweenScale : TweenBase
{
	public Vector3 from = Vector3.one;
	public Vector3 to = Vector3.one;

	public Vector3 value {
		get {
			return transform.localScale;
		}
		set {
			transform.localScale = value;
		}
	}

	/// <summary>
	/// Call Tweening Logic.
	/// </summary>
	/// <param name="factor">Factor.</param>
	/// <param name="isFinished">If set to <c>true</c> is finished.</param>
	protected override void OnUpdate (float factor, bool isFinished)
	{
		value = from * (1f - factor) + to * factor;
	}

	/// <summary>
	/// Begin tweening operation.
	/// </summary>
	/// <param name="scale">Final Scale value</param>
	static public TweenScale Begin (GameObject go, float duration, Vector3 scale)
	{
		TweenScale comp = TweenBase.Begin<TweenScale> (go, duration);
		comp.from = comp.value;
		comp.to = scale;

		if (duration <= 0f) {
			comp.OnFactorModified (1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	/// <summary>
	/// Set 'from' value to current value.
	/// </summary>
	[ContextMenu ("Set 'From' to current value")]
	public override void SetStartToCurrentValue ()
	{
		from = value;
	}

	/// <summary>
	/// Set 'to' value to the current value.
	/// </summary>
	[ContextMenu ("Set 'To' to current value")]
	public override void SetEndToCurrentValue ()
	{
		to = value;
	}

	/// <summary>
	/// Sets current value to 'from' value.
	/// </summary>
	[ContextMenu ("Assume value of 'From'")]
	public override void SetCurrentValueToStart ()
	{
		value = from;
	}

	/// <summary>
	/// Sets current value to 'to' value.
	/// </summary>
	[ContextMenu ("Assume value of 'To'")]
	public override void SetCurrentValueToEnd ()
	{
		value = to;
	}
}
