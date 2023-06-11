using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(TweenRotation))]
public class TweenRotationEditor : TweenBaseEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space (6f);
		TweenEditorUtility.SetLabelWidth (120f);

		TweenRotation tw = target as TweenRotation;
		GUI.changed = false;

		Vector3 from = EditorGUILayout.Vector3Field ("From", tw.from);
		Vector3 to = EditorGUILayout.Vector3Field ("To", tw.to);

		if (GUI.changed) {
			TweenEditorUtility.RegisterUndo ("Tween Change", tw);
			tw.from = from;
			tw.to = to;
			TweenEditorUtility.SetDirty (tw);
		}

		DrawCommonProperties ();
	}
}
