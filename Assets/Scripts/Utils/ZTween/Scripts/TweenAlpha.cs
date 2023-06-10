using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween GameObject's alpha.
/// </summary>
[AddComponentMenu ("ZTween/Tween Alpha")]
public class TweenAlpha : TweenBase
{
	[Range (0f, 1f)] public float from = 1f;
	[Range (0f, 1f)] public float to = 1f;

	bool mCached = false;
	Material mMat;
	SpriteRenderer mSr;
	CanvasGroup mGroup;
	MaskableGraphic mImg;
	Text txt;

	void Cache ()
	{
		mCached = true;
		mGroup = GetComponent<CanvasGroup> ();
		if (mGroup == null) {
			txt = GetComponent<Text> ();
		}
		if (txt == null) {
			mSr = GetComponent<SpriteRenderer> ();
		}
		if (mSr == null) {
			mImg = GetComponent<MaskableGraphic> ();
		}
		if (mImg == null) {
			Renderer ren = GetComponent<Renderer> ();
			if (ren != null)
				mMat = ren.material;
		}
	}

	public float value {
		get {
			if (!mCached)
				Cache ();
			if (mGroup != null)
				return mGroup.alpha;
			else if (txt != null)
				return txt.color.a;
			else if (mSr != null)
				return mSr.color.a;
			else if (mImg != null)
				return mImg.color.a;
			return mMat != null ? mMat.color.a : 1f;
		}
		set {
			if (!mCached)
				Cache ();
			if (mGroup != null) {
				mGroup.alpha = value;
			} else if (txt != null) {
				Color c = txt.color;
				c.a = value;
				txt.color = c;
			} else if (mSr != null) {
				Color c = mSr.color;
				c.a = value;
				mSr.color = c;
			} else if (mImg != null) {
				Color c = mImg.color;
				c.a = value;
				mImg.color = c;
			} else if (mMat != null) {
				Color c = mMat.color;
				c.a = value;
				mMat.color = c;
			}
		}
	}

	/// <summary>
	/// Call Tweening Logic.
	/// </summary>
	/// <param name="factor">Factor.</param>
	/// <param name="isFinished">If set to <c>true</c> is finished.</param>
	protected override void OnUpdate (float factor, bool isFinished)
	{
		value = Mathf.Lerp (from, to, factor);
	}

	/// <summary>
	/// Begin tweening operation.
	/// </summary>
	/// <param name="alpha">Final alpha value</param>
	static public TweenAlpha Begin (GameObject go, float duration, float alpha)
	{
		TweenAlpha comp = TweenBase.Begin<TweenAlpha> (go, duration);
		comp.from = comp.value;
		comp.to = alpha;

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
