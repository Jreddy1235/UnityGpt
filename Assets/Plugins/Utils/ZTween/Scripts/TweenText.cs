using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween Text with typewriter effect
/// </summary>
[AddComponentMenu ("ZTween/Tween Text")]
public class TweenText : TweenBase
{
	public string from = "";
	public string to = "";

	bool mCached = false;
	Text mText;

	void Cache ()
	{
		mCached = true;

		mText = GetComponent<Text> ();
		if (mText != null)
			return;
	}

	public string value {
		get {
			if (!mCached)
				Cache ();
			if (mText != null)
				return mText.text;
			return "";
		}
		set {
			if (!mCached)
				Cache ();
			if (mText != null)
				mText.text = value;
		}
	}

	/// <summary>
	/// Call Tweening Logic.
	/// </summary>
	/// <param name="factor">Factor.</param>
	/// <param name="isFinished">If set to <c>true</c> is finished.</param>
	protected override void OnUpdate (float factor, bool isFinished)
	{
		if (!to.Contains (from))
			return;
		string myString = to.Remove (0, from.Length);
		int count = (int)(myString.Length * factor);
		value = from + myString.Substring (0, count);
	}

	/// <summary>
	/// Begin tweening operation.
	/// </summary>
	/// <param name="text">Final Text value</param>
	static public TweenText Begin (GameObject go, float duration, string text)
	{
		TweenText comp = TweenBase.Begin<TweenText> (go, duration);
		comp.from = comp.value;
		comp.to = text;

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
