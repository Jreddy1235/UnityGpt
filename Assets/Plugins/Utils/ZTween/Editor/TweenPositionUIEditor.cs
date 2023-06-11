using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(TweenPositionUI))]
public class TweenPositionUIEditor : TweenBaseEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space (6f);
		TweenEditorUtility.SetLabelWidth (120f);

		TweenPositionUI tw = target as TweenPositionUI;
		GUI.changed = false;
		Vector3 fromPosition = Vector3.zero;
		Vector3 toPosition = Vector3.zero;
		Transform from = null, to = null;

		EditorGUILayout.BeginHorizontal ();
		tw.absoluteFrom = EditorGUILayout.Toggle ("Absolute From", tw.absoluteFrom);
		tw.absoluteTo = EditorGUILayout.Toggle ("Absolute To", tw.absoluteTo);
		EditorGUILayout.EndHorizontal ();

		if (tw.absoluteFrom)
			fromPosition = EditorGUILayout.Vector3Field ("From", tw.fromPosition);
		else
			from = EditorGUILayout.ObjectField ("From", tw.from, typeof(Transform), true) as Transform;

		if (tw.absoluteTo)
			toPosition = EditorGUILayout.Vector3Field ("To", tw.toPosition);
		else
			to = EditorGUILayout.ObjectField ("To", tw.to, typeof(Transform), true) as Transform;

		if (GUI.changed) {
			TweenEditorUtility.RegisterUndo ("Tween Change", tw);
			tw.fromPosition = fromPosition;
			tw.toPosition = toPosition;
			tw.from = from;
			tw.to = to;
			TweenEditorUtility.SetDirty (tw);
		}

		DrawCommonProperties ();
	}
}
