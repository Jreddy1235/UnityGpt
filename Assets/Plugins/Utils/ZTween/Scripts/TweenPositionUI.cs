using UnityEngine;

/// <summary>
/// Tween GameObject's position  specially for UI.
/// </summary>
[AddComponentMenu ("ZTween/Tween Position UI")]
public class TweenPositionUI : TweenBase
{
	public bool absoluteFrom;
	public Vector3 fromPosition;
	public bool absoluteTo;
	public Vector3 toPosition;
	public Transform from;

	public Vector3 From {
		get {
			if (absoluteFrom || from == transform)
				return fromPosition;
			return from.position;
		}
	}

	public Transform to;

	public Vector3 To {
		get {
			if (absoluteTo || to == transform)
				return toPosition;
			return to.position;
		}
	}

	public Vector3 value {
		get {
			return transform.position;
		}
		set {
			transform.position = value;
		}
	}

	void Awake ()
	{
		InitPositions ();
	}

	private bool isInit;

	/// <summary>
	/// Initializes absolute positions for from and to
	/// </summary>
	public void InitPositions ()
	{
		if (isInit)
			return;
		if (from != null && from == transform) {
			fromPosition = from.position;
		}
		if (to != null && to == transform) {
			toPosition = to.position;
		}
		isInit = true;
	}

	/// <summary>
	/// Call Tweening Logic.
	/// </summary>
	/// <param name="factor">Factor.</param>
	/// <param name="isFinished">If set to <c>true</c> is finished.</param>
	protected override void OnUpdate (float factor, bool isFinished)
	{
		value = From * (1f - factor) + To * factor;
	}

	/// <summary>
	/// Begin tweening operation.
	/// </summary>
	static public TweenPositionUI Begin (GameObject go, float duration, Transform from, Transform to)
	{
		TweenPositionUI comp = TweenBase.Begin<TweenPositionUI> (go, duration);
		comp.from = from;
		comp.to = to;
		comp.isInit = false;
		comp.InitPositions ();

		if (duration <= 0f) {
			comp.OnFactorModified (1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	/// <summary>
	/// Begin tweening operation.
	/// </summary>
	static public TweenPositionUI Begin (GameObject go, float duration, Vector3 from, Vector3 to)
	{
		TweenPositionUI comp = TweenBase.Begin<TweenPositionUI> (go, duration);
		comp.fromPosition = from;
		comp.toPosition = to;

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
		InitPositions ();

		if (absoluteFrom)
			fromPosition = value;
		else if (from != null)
			from.position = value;
	}

	/// <summary>
	/// Set 'to' value to the current value.
	/// </summary>
	[ContextMenu ("Set 'To' to current value")]
	public override void SetEndToCurrentValue ()
	{
		InitPositions ();

		if (absoluteTo)
			toPosition = value;
		else if (to != null)
			to.position = value;
	}

	/// <summary>
	/// Sets current value to 'from' value.
	/// </summary>
	[ContextMenu ("Assume value of 'From'")]
	public override void SetCurrentValueToStart ()
	{
		if (absoluteFrom)
			value = fromPosition;
		else if (from != null)
			value = from.position;
	}

	/// <summary>
	/// Sets current value to 'to' value.
	/// </summary>
	[ContextMenu ("Assume value of 'To'")]
	public override void SetCurrentValueToEnd ()
	{
		if (absoluteTo)
			value = toPosition;
		else if (to != null)
			value = to.position;
	}
}
