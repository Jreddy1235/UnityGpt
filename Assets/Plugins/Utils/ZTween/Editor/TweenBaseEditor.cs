using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor (typeof(TweenBase), true)]
[CanEditMultipleObjects]
public class TweenBaseEditor : Editor
{
	private string[] easingNames;

	void OnEnable ()
	{
		easingNames = CurvePresetLibrary.GetNames ();
		easingNames = new string[]{ "Custom" }.Concat (easingNames).ToArray ();
	}

	public override void OnInspectorGUI ()
	{
		GUILayout.Space (6f);
		TweenEditorUtility.SetLabelWidth (110f);
		base.OnInspectorGUI ();
		DrawCommonProperties ();
	}

	protected void DrawCommonProperties ()
	{
		TweenBase tw = target as TweenBase;

		if (TweenEditorUtility.DrawHeader ("Tween Properties")) {
			TweenEditorUtility.BeginContents ();
			TweenEditorUtility.SetLabelWidth (110f);

			GUI.changed = false;

			TweenBase.Style style = (TweenBase.Style)EditorGUILayout.EnumPopup ("Play Style", tw.style);
			int selectedEasing = EditorGUILayout.Popup ("Easing Curve", tw.selectedEasing, easingNames);

			EditorGUI.BeginChangeCheck ();
			AnimationCurve curve = EditorGUILayout.CurveField ("Animation Curve", tw.animationCurve, GUILayout.Width (220f), GUILayout.Height (80f));
			if (EditorGUI.EndChangeCheck ()) {
				tw.selectedEasing = selectedEasing = 0;
			}

			GUILayout.BeginHorizontal ();
			float dur = EditorGUILayout.FloatField ("Duration", tw.duration, GUILayout.Width (170f));
			GUILayout.Label ("seconds");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			float del = EditorGUILayout.FloatField ("Delay", tw.delay, GUILayout.Width (170f));
			GUILayout.Label ("seconds");
			GUILayout.EndHorizontal ();

			bool ts = EditorGUILayout.Toggle ("Ignore TimeScale", tw.ignoreTimeScale);

			if (GUI.changed) {
				TweenEditorUtility.RegisterUndo ("Tween Change", tw);
				tw.style = style;
				tw.ignoreTimeScale = ts;
				tw.duration = dur;
				tw.delay = del;
				tw.animationCurve = curve;
				if (selectedEasing != tw.selectedEasing) {
					tw.selectedEasing = selectedEasing;
					if (selectedEasing > 0)
						tw.animationCurve.keys = CurvePresetLibrary.GetPreset<AnimationCurve> (selectedEasing - 1).keys;
				}
				TweenEditorUtility.SetDirty (tw);
			}
			
			serializedObject.Update ();
			EditorGUILayout.PropertyField (serializedObject.FindProperty ("onFinished"));
			serializedObject.ApplyModifiedProperties ();
			TweenEditorUtility.EndContents ();
		}

	}
}
