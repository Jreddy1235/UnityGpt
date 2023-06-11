using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(TweenAlpha))]
public class TweenAlphaEditor : TweenBaseEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space (6f);
		TweenEditorUtility.SetLabelWidth (120f);

		TweenAlpha tw = target as TweenAlpha;
		GUI.changed = false;

		float from = EditorGUILayout.Slider ("From", tw.from, 0f, 1f);
		float to = EditorGUILayout.Slider ("To", tw.to, 0f, 1f);

		if (GUI.changed) {
			TweenEditorUtility.RegisterUndo ("Tween Change", tw);
			tw.from = from;
			tw.to = to;
			TweenEditorUtility.SetDirty (tw);
		}

		DrawCommonProperties ();
	}
}
