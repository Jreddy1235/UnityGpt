using UnityEngine;

/// <summary>
/// Tween GameObject's rotation.
/// </summary>
[AddComponentMenu ("ZTween/Tween Rotation")]
public class TweenRotation : TweenBase
{
	public Vector3 from;
	public Vector3 to;
	public bool quaternionLerp = false;

	public Quaternion value {
		get { 
			return transform.localRotation;
		} 
		set { 
			transform.localRotation = value;
		}
	}

	/// <summary>
	/// Call Tweening Logic.
	/// </summary>
	/// <param name="factor">Factor.</param>
	/// <param name="isFinished">If set to <c>true</c> is finished.</param>
	protected override void OnUpdate (float factor, bool isFinished)
	{
		value = quaternionLerp ? Quaternion.Slerp (Quaternion.Euler (from), Quaternion.Euler (to), factor) :
			Quaternion.Euler (new Vector3 (
			Mathf.Lerp (from.x, to.x, factor),
			Mathf.Lerp (from.y, to.y, factor),
			Mathf.Lerp (from.z, to.z, factor)));
	}

	/// <summary>
	/// Begin tweening operation.
	/// </summary>
	/// <param name="rot">Final Rotation value</param>
	static public TweenRotation Begin (GameObject go, float duration, Quaternion rot)
	{
		TweenRotation comp = TweenBase.Begin<TweenRotation> (go, duration);
		comp.from = comp.value.eulerAngles;
		comp.to = rot.eulerAngles;

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
		from = value.eulerAngles;
	}

	/// <summary>
	/// Set 'to' value to the current value.
	/// </summary>
	[ContextMenu ("Set 'To' to current value")]
	public override void SetEndToCurrentValue ()
	{
		to = value.eulerAngles;
	}

	/// <summary>
	/// Sets current value to 'from' value.
	/// </summary>
	[ContextMenu ("Assume value of 'From'")]
	public override void SetCurrentValueToStart ()
	{
		value = Quaternion.Euler (from);
	}

	/// <summary>
	/// Sets current value to 'to' value.
	/// </summary>
	[ContextMenu ("Assume value of 'To'")]
	public override void SetCurrentValueToEnd ()
	{
		value = Quaternion.Euler (to);
	}
}
