using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.IO;

public class CurvePresetLibrary
{
	public static Type presetLibraryType = GetType ("UnityEditor.PresetLibrary");
	public static UnityEngine.Object presetLibraryObject = GetObject ("EasingCurves");

	static Type GetType (string typeName)
	{
		return System.Reflection.Assembly.Load ("UnityEditor.dll").GetType (typeName);
	}

	static UnityEngine.Object GetObject (string objectName)
	{
		UnityEngine.Object obj = null;
		var allPaths = AssetDatabase.GetAllAssetPaths ();
		foreach (var path in allPaths) {
			if (!File.Exists (path + "/EasingCurves.curves"))
				continue;
			obj = AssetDatabase.LoadAssetAtPath (path + "/EasingCurves.curves", presetLibraryType);
			if (obj != null)
				break;
		}
		return obj;
	}

	public static string[] GetNames ()
	{
		int count = Count ();
		MethodInfo info = presetLibraryType.GetMethod ("GetName", new []{ typeof(int) });
		string[] names = new string[count];
		for (int i = 0; i < count; i++) {
			object obj = info.Invoke (presetLibraryObject, new object[]{ i });
			names [i] = obj != null ? obj.ToString () : "";
		}
		return names;
	}

	public static int Count ()
	{
		MethodInfo info = presetLibraryType.GetMethod ("Count");
		object obj = info.Invoke (presetLibraryObject, new object[0]);
		return obj != null ? (int)obj : 0;
	}

	public static T GetPreset<T> (int index)
	{
		MethodInfo info = presetLibraryType.GetMethod ("GetPreset", new []{ typeof(int) });
		System.Object obj = info.Invoke (presetLibraryObject, new object[]{ index });
		return obj != null ? (T)obj : default(T);
	}
}
