using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween GameObject's color.
/// </summary>
[AddComponentMenu ("ZTween/Tween Color")]
public class TweenColor : TweenBase
{
	public Color from = Color.white;
	public Color to = Color.white;

	bool mCached = false;
	Image mImage;
	Text mText;
	Material mMat;
	Light mLight;
	SpriteRenderer mSr;

	void Cache ()
	{
		mCached = true;
		mImage = GetComponent<Image> ();
		if (mImage != null)
			return;

		mText = GetComponent<Text> ();
		if (mText != null)
			return;
		
		mSr = GetComponent<SpriteRenderer> ();
		if (mSr != null)
			return;

		Renderer ren = GetComponent<Renderer> ();
		if (ren != null) {
			mMat = ren.material;
			return;
		}

		mLight = GetComponent<Light> ();
		if (mLight == null)
			mImage = GetComponentInChildren<Image> ();
	}

	public Color value {
		get {
			if (!mCached)
				Cache ();
			if (mImage != null)
				return mImage.color;
			if (mText != null)
				return mText.color;
			if (mMat != null)
				return mMat.color;
			if (mSr != null)
				return mSr.color;
			if (mLight != null)
				return mLight.color;
			return Color.black;
		}
		set {
			if (!mCached)
				Cache ();
			if (mImage != null)
				mImage.color = value;
			if (mText != null)
				mText.color = value;
			else if (mMat != null)
				mMat.color = value;
			else if (mSr != null)
				mSr.color = value;
			else if (mLight != null) {
				mLight.color = value;
				mLight.enabled = (value.r + value.g + value.b) > 0.01f;
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
		value = Color.Lerp (from, to, factor);
	}

	/// <summary>
	/// Begin tweening operation.
	/// </summary>
	/// <param name="color">Final color value</param>
	static public TweenColor Begin (GameObject go, float duration, Color color)
	{
		TweenColor comp = TweenBase.Begin<TweenColor> (go, duration);
		comp.from = comp.value;
		comp.to = color;

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
