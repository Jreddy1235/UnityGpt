using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(TweenColor))]
public class TweenColorEditor : TweenBaseEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space (6f);
		TweenEditorUtility.SetLabelWidth (120f);

		TweenColor tw = target as TweenColor;
		GUI.changed = false;

		Color from = EditorGUILayout.ColorField ("From", tw.from);
		Color to = EditorGUILayout.ColorField ("To", tw.to);

		if (GUI.changed) {
			TweenEditorUtility.RegisterUndo ("Tween Change", tw);
			tw.from = from;
			tw.to = to;
			TweenEditorUtility.SetDirty (tw);
		}

		DrawCommonProperties ();
	}
}
