using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Common editor utilities
/// </summary>
public static class TweenEditorUtility
{
	static Texture2D mBackdropTex;
	static Texture2D mContrastTex;
	static Texture2D mGradientTex;

	/// <summary>
	/// Returns a blank white texture.
	/// </summary>
	public static Texture2D blankTexture {
		get {
			return EditorGUIUtility.whiteTexture;
		}
	}

	/// <summary>
	/// Returns a checker texture
	/// </summary>

	public static Texture2D backdropTexture {
		get {
			if (mBackdropTex == null)
				mBackdropTex = CreateCheckerTex (
					new Color (0.1f, 0.1f, 0.1f, 0.5f),
					new Color (0.2f, 0.2f, 0.2f, 0.5f));
			return mBackdropTex;
		}
	}

	/// <summary>
	/// Returns a contrast checker texture
	/// </summary>

	public static Texture2D contrastTexture {
		get {
			if (mContrastTex == null)
				mContrastTex = CreateCheckerTex (
					new Color (0f, 0.0f, 0f, 0.5f),
					new Color (1f, 1f, 1f, 0.5f));
			return mContrastTex;
		}
	}

	/// <summary>
	/// Returns a gradient texture
	/// </summary>
	public static Texture2D gradientTexture {
		get {
			if (mGradientTex == null)
				mGradientTex = CreateGradientTex ();
			return mGradientTex;
		}
	}

	/// <summary>
	/// Create a checker texture
	/// </summary>
	static Texture2D CreateCheckerTex (Color c0, Color c1)
	{
		Texture2D tex = new Texture2D (16, 16);
		tex.name = "[Generated] Checker Texture";
		tex.hideFlags = HideFlags.DontSave;

		for (int y = 0; y < 8; ++y)
			for (int x = 0; x < 8; ++x)
				tex.SetPixel (x, y, c1);
		for (int y = 8; y < 16; ++y)
			for (int x = 0; x < 8; ++x)
				tex.SetPixel (x, y, c0);
		for (int y = 0; y < 8; ++y)
			for (int x = 8; x < 16; ++x)
				tex.SetPixel (x, y, c0);
		for (int y = 8; y < 16; ++y)
			for (int x = 8; x < 16; ++x)
				tex.SetPixel (x, y, c1);

		tex.Apply ();
		tex.filterMode = FilterMode.Point;
		return tex;
	}

	/// <summary>
	/// Create a gradient texture
	/// </summary>

	static Texture2D CreateGradientTex ()
	{
		Texture2D tex = new Texture2D (1, 16);
		tex.name = "[Generated] Gradient Texture";
		tex.hideFlags = HideFlags.DontSave;

		Color c0 = new Color (1f, 1f, 1f, 0f);
		Color c1 = new Color (1f, 1f, 1f, 0.4f);

		for (int i = 0; i < 16; ++i) {
			float f = Mathf.Abs ((i / 15f) * 2f - 1f);
			f *= f;
			tex.SetPixel (0, i, Color.Lerp (c0, c1, f));
		}

		tex.Apply ();
		tex.filterMode = FilterMode.Bilinear;
		return tex;
	}

	/// <summary>
	/// Draws the tiled texture
	/// </summary>
	public static void DrawTiledTexture (Rect rect, Texture tex)
	{
		GUI.BeginGroup (rect);
		{
			int width = Mathf.RoundToInt (rect.width);
			int height = Mathf.RoundToInt (rect.height);

			for (int y = 0; y < height; y += tex.height) {
				for (int x = 0; x < width; x += tex.width) {
					GUI.DrawTexture (new Rect (x, y, tex.width, tex.height), tex);
				}
			}
		}
		GUI.EndGroup ();
	}

	/// <summary>
	/// Draws outline around the specified rect
	/// </summary>
	public static void DrawOutline (Rect rect)
	{
		if (Event.current.type == EventType.Repaint) {
			Texture2D tex = contrastTexture;
			GUI.color = Color.white;
			DrawTiledTexture (new Rect (rect.xMin, rect.yMax, 1f, -rect.height), tex);
			DrawTiledTexture (new Rect (rect.xMax, rect.yMax, 1f, -rect.height), tex);
			DrawTiledTexture (new Rect (rect.xMin, rect.yMin, rect.width, 1f), tex);
			DrawTiledTexture (new Rect (rect.xMin, rect.yMax, rect.width, 1f), tex);
		}
	}

	/// <summary>
	/// Draws outline around the specified rect with specified color
	/// </summary>
	public static void DrawOutline (Rect rect, Color color)
	{
		if (Event.current.type == EventType.Repaint) {
			Texture2D tex = blankTexture;
			GUI.color = color;
			GUI.DrawTexture (new Rect (rect.xMin, rect.yMin, 1f, rect.height), tex);
			GUI.DrawTexture (new Rect (rect.xMax, rect.yMin, 1f, rect.height), tex);
			GUI.DrawTexture (new Rect (rect.xMin, rect.yMin, rect.width, 1f), tex);
			GUI.DrawTexture (new Rect (rect.xMin, rect.yMax, rect.width, 1f), tex);
			GUI.color = Color.white;
		}
	}

	/// <summary>
	/// Draws large Foldouts like in shuriken particle system
	/// </summary>
	public static bool Foldout (string title, bool display)
	{
		var style = new GUIStyle ("ShurikenModuleTitle");
		style.font = new GUIStyle (EditorStyles.label).font;
		style.border = new RectOffset (15, 7, 4, 4);
		style.fixedHeight = 22;
		style.contentOffset = new Vector2 (20f, -2f);

		var rect = GUILayoutUtility.GetRect (16f, 22f, style);
		GUI.Box (rect, title, style);

		var e = Event.current;

		var toggleRect = new Rect (rect.x + 4f, rect.y + 2f, 13f, 13f);
		if (e.type == EventType.Repaint) {
			EditorStyles.foldout.Draw (toggleRect, false, false, display, false);
		}

		if (e.type == EventType.MouseDown && rect.Contains (e.mousePosition)) {
			display = !display;
			e.Use ();
		}

		return display;
	}

	public struct IntVector
	{
		public int x;
		public int y;
	}

	/// <summary>
	/// Draws Integer based Vector field
	/// </summary>
	public static IntVector IntPair (string prefix, string leftCaption, string rightCaption, int x, int y)
	{
		GUILayout.BeginHorizontal ();

		if (string.IsNullOrEmpty (prefix)) {
			GUILayout.Space (82f);
		} else {
			GUILayout.Label (prefix, GUILayout.Width (74f));
		}

		EditorGUIUtility.labelWidth = 48f;

		IntVector retVal;
		retVal.x = EditorGUILayout.IntField (leftCaption, x, GUILayout.MinWidth (30f));
		retVal.y = EditorGUILayout.IntField (rightCaption, y, GUILayout.MinWidth (30f));

		EditorGUIUtility.labelWidth = 80f;

		GUILayout.EndHorizontal ();
		return retVal;
	}

	/// <summary>
	/// Adds space of single line to editor gui
	/// </summary>
	public static void DrawPadding ()
	{
		GUILayout.Space (18f);
	}

	/// <summary>
	/// Draws the separator in editor gui
	/// </summary>
	public static void DrawSeparator ()
	{
		GUILayout.Space (12f);

		if (Event.current.type == EventType.Repaint) {
			Texture2D tex = blankTexture;
			Rect rect = GUILayoutUtility.GetLastRect ();
			GUI.color = new Color (0f, 0f, 0f, 0.25f);
			GUI.DrawTexture (new Rect (0f, rect.yMin + 6f, Screen.width, 4f), tex);
			GUI.DrawTexture (new Rect (0f, rect.yMin + 6f, Screen.width, 1f), tex);
			GUI.DrawTexture (new Rect (0f, rect.yMin + 9f, Screen.width, 1f), tex);
			GUI.color = Color.white;
		}
	}

	/// <summary>
	/// Draws mini version of foldable header
	/// </summary>
	public static bool DrawMinimalisticHeader (string text)
	{
		return DrawHeader (text, text, false, true);
	}

	/// <summary>
	/// Draws detailed version of foldable header
	/// </summary>
	public static bool DrawHeader (string text)
	{
		return DrawHeader (text, text, false, false);
	}

	/// <summary>
	/// Draws detailed version of foldable header
	/// </summary>
	/// <param name="key">Key to be stored in editor prefs</param>
	public static bool DrawHeader (string text, string key)
	{
		return DrawHeader (text, key, false, false);
	}

	/// <summary>
	/// Draws foldable header
	/// </summary>
	/// <param name="detailed">if true, draw detailed header</param>
	public static bool DrawHeader (string text, bool detailed)
	{
		return DrawHeader (text, text, detailed, !detailed);
	}

	/// <summary>
	/// Draws detailed version of foldable header
	/// </summary>
	/// <param name="key">Key to be stored in editor prefs</param>
	public static bool DrawHeader (string text, string key, bool forceOn, bool minimalistic)
	{
		bool state = EditorPrefs.GetBool (key, true);

		if (!minimalistic)
			GUILayout.Space (3f);
		if (!forceOn && !state)
			GUI.backgroundColor = new Color (0.8f, 0.8f, 0.8f);
		GUILayout.BeginHorizontal ();
		GUI.changed = false;

		if (minimalistic) {
			if (state)
				text = "\u25BC" + (char)0x200a + text;
			else
				text = "\u25BA" + (char)0x200a + text;

			GUILayout.BeginHorizontal ();
			GUI.contentColor = EditorGUIUtility.isProSkin ? new Color (1f, 1f, 1f, 0.7f) : new Color (0f, 0f, 0f, 0.7f);
			if (!GUILayout.Toggle (true, text, "PreToolbar2", GUILayout.MinWidth (20f)))
				state = !state;
			GUI.contentColor = Color.white;
			GUILayout.EndHorizontal ();
		} else {
			text = "<b><size=11>" + text + "</size></b>";
			if (state)
				text = "\u25BC " + text;
			else
				text = "\u25BA " + text;
			if (!GUILayout.Toggle (true, text, "dragtab", GUILayout.MinWidth (20f)))
				state = !state;
		}

		if (GUI.changed)
			EditorPrefs.SetBool (key, state);

		if (!minimalistic)
			GUILayout.Space (2f);
		GUILayout.EndHorizontal ();
		GUI.backgroundColor = Color.white;
		if (!forceOn && !state)
			GUILayout.Space (3f);
		return state;
	}

	/// <summary>
	/// Begin drawing the contents
	/// </summary>
	public static void BeginContents ()
	{
		BeginContents (false);
	}

	static bool mEndHorizontal = false;

	/// <summary>
	/// Begin drawing the contents without base
	/// </summary>
	public static void BeginContents (bool minimalistic)
	{
		if (!minimalistic) {
			mEndHorizontal = true;
			GUILayout.BeginHorizontal ();
			EditorGUILayout.BeginHorizontal ("TextArea", GUILayout.MinHeight (10f));
		} else {
			mEndHorizontal = false;
			EditorGUILayout.BeginHorizontal (GUILayout.MinHeight (10f));
			GUILayout.Space (10f);
		}
		GUILayout.BeginVertical ();
		GUILayout.Space (2f);
	}

	/// <summary>
	/// End drawing the contents.
	/// </summary>
	public static void EndContents ()
	{
		GUILayout.Space (3f);
		GUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();

		if (mEndHorizontal) {
			GUILayout.Space (3f);
			GUILayout.EndHorizontal ();
		}

		GUILayout.Space (3f);
	}

	/// <summary>
	/// Sets the width of the label for editor gui
	/// </summary>
	public static void SetLabelWidth (float width)
	{
		EditorGUIUtility.labelWidth = width;
	}

	/// <summary>
	/// Registers undo for specified object
	/// </summary>
	public static void RegisterUndo (string name, params Object[] objects)
	{
		if (objects != null && objects.Length > 0) {
			UnityEditor.Undo.RecordObjects (objects, name);

			foreach (Object obj in objects) {
				if (obj == null)
					continue;
				TweenEditorUtility.SetDirty (obj);
			}
		}
	}

	/// <summary>
	/// Registers undo for specified object
	/// </summary>
	public static void RegisterUndo (UnityEngine.Object obj, string name)
	{
		UnityEditor.Undo.RecordObject (obj, name);
		TweenEditorUtility.SetDirty (obj);
	}

	/// <summary>
	/// Marks the specified object as dirty
	/// </summary>
	public static void SetDirty (UnityEngine.Object obj)
	{
		if (obj) {
			UnityEditor.EditorUtility.SetDirty (obj);
		}
	}
}
