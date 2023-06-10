using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(TweenGroup))]
public class TweenGroupEditor : Editor
{
    private TweenGroup tg;
    private ReorderableList tweenList, groupList;
    private List<TweenInfo> tweensInfo = new List<TweenInfo>();
    private Dictionary<TweenInfoArray, TweenInfoDrawer> tweenInfoDrawers;
    private bool isDisplay;
    private static int tabIndex = 0;

    void OnEnable()
    {
        tg = (TweenGroup)target;
        tabIndex = Application.isPlaying ? 2 : 0;
        tweenInfoDrawers = new Dictionary<TweenInfoArray, TweenInfoDrawer>();
        for (int i = 0; i < tg.tweensInfo.Count; i++)
        {
            tweenInfoDrawers[tg.tweensInfo[i]] = new TweenInfoDrawer();
        }
        isDisplay = EditorPrefs.GetBool("TweensFoldOut", true);
        InitList();
    }

    private void InitList()
    {
        tweenList = new ReorderableList(tweensInfo, typeof(TweenInfo), true, true, true, true);
        tweenList.drawElementCallback = DrawCreateElement;
        tweenList.drawHeaderCallback = (Rect rect) =>
        {
            var newRect = new Rect(rect.x + 10, rect.y, rect.width - 10, rect.height);
            isDisplay = EditorGUI.Foldout(newRect, isDisplay, "Tween Info");
            EditorPrefs.SetBool("TweensFoldOut", isDisplay);
        };
        tweenList.elementHeightCallback = (int indexer) =>
        {
            return isDisplay ? EditorGUIUtility.singleLineHeight * 3 : 0;
        };

        groupList = new ReorderableList(serializedObject, serializedObject.FindProperty("groups"), true, true, true, true);
        groupList.drawElementCallback = DrawGroupElement;
        groupList.drawHeaderCallback = (Rect rect) =>
        {
            var newRect = new Rect(rect.x + 10, rect.y, rect.width - 10, rect.height);
            isDisplay = EditorGUI.Foldout(newRect, isDisplay, "Group Info");
            EditorPrefs.SetBool("TweensFoldOut", isDisplay);
        };
        groupList.elementHeightCallback = (int indexer) =>
        {
            return isDisplay ? EditorGUIUtility.singleLineHeight * 1.2f : 0;
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawTabs();

        switch (tabIndex)
        {
            case 0:
                DrawCreate();
                break;
            case 1:
                DrawArrange();
                break;
            case 2:
                DrawRun();
                break;
        }

        if (GUI.changed)
        {
            EditorSceneManager.MarkAllScenesDirty();
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawRun()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width - 150);
        EditorGUILayout.LabelField("Key", GUILayout.MaxWidth(30));
        tg.key = EditorGUILayout.TextField(tg.key);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Play Forward"))
        {
            tg.Init();
            tg.PlayForward();
        }
        if (GUILayout.Button("Play Reverse"))
        {
            tg.Init();
            tg.PlayReverse();
        }
        EditorGUILayout.EndHorizontal();

        if (TweenEditorUtility.DrawHeader("Play List"))
        {
            TweenEditorUtility.BeginContents();
            if (tg.hasSubGroups)
            {
                for (int i = 0; i < tg.groups.Count; i++)
                {
                    TweenEditorUtility.Foldout(i.ToString(), true);
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Play Forward"))
                    {
                        tg.groups[i].PlayForward();
                    }
                    if (GUILayout.Button("Reset"))
                    {
                        tg.groups[i].ResetAll();
                    }
                    if (GUILayout.Button("Play Reverse"))
                    {
                        tg.currentIndex = i;
                        tg.groups[i].PlayReverse();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                for (int i = 0; i < tg.tweensInfo.Count; i++)
                {
                    TweenEditorUtility.Foldout(i.ToString(), true);
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Play Forward"))
                    {
                        tg.currentIndex = i;
                        tg.PlayForward();
                    }
                    if (GUILayout.Button("Reset"))
                    {
                        tg.Reset(i);
                    }
                    if (GUILayout.Button("Play Reverse"))
                    {
                        tg.currentIndex = i;
                        tg.PlayReverse();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            TweenEditorUtility.EndContents();
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("onFinished"));
        tg.autoPlayNext = EditorGUILayout.ToggleLeft("Auto Play", tg.autoPlayNext);
    }

    private void InsertAt(int index, TweenInfoArray tweenInfoArray)
    {
        if (tg.tweensInfo.Remove(tweenInfoArray))
            tg.tweensInfo.Insert(index, tweenInfoArray);
    }

    private void DrawTweensInfo(int index, TweenInfoArray tweenInfoArray)
    {
        if (TweenEditorUtility.DrawHeader(index.ToString(), true))
        {
            TweenEditorUtility.BeginContents();

            System.Action<int> onMoveTo = (x) =>
            {
                GenericMenu menu = new GenericMenu();
                for (int i = 0; i < tg.tweensInfo.Count; i++)
                {
                    int k = i;
                    menu.AddItem(new GUIContent(k.ToString()), k == index, () =>
                    {
                        var element = tweenInfoArray.list[x];
                        var tempList = tweenInfoArray.ToList();
                        tempList.Remove(element);
                        if (tempList.Count == 0)
                            tg.tweensInfo.Remove(tweenInfoArray);
                        else
                            tweenInfoArray.list = tempList.ToList();
                        tempList = tg.tweensInfo[k].list.ToList();
                        tempList.Add(element);
                        tg.tweensInfo[k].list = tempList.ToList();
                        tweenInfoDrawers.Clear();
                    });
                }
                menu.ShowAsContext();
            };

            if (!tweenInfoDrawers.ContainsKey(tweenInfoArray))
                tweenInfoDrawers[tweenInfoArray] = new TweenInfoDrawer();
            tweenInfoDrawers[tweenInfoArray].Draw(tweenInfoArray, onMoveTo);

            GUI.enabled = true;
            GUILayout.Space(-12f);
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(index <= 0);
            if (GUILayout.Button("\u25B2"))
            {
                InsertAt(index - 1, tweenInfoArray);
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(index >= tg.tweensInfo.Count - 1);
            if (GUILayout.Button("\u25BC"))
            {
                InsertAt(index + 1, tweenInfoArray);
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.Space(Screen.width - 120);
            EditorGUILayout.EndHorizontal();
            TweenEditorUtility.EndContents();
        }
    }

    private void DrawArrange()
    {
        tg.hasSubGroups = EditorGUILayout.ToggleLeft("Has SubGroups", tg.hasSubGroups);
        if (tg.hasSubGroups)
        {
            groupList.DoLayoutList();
        }
        else
        {
            for (int i = 0; i < tg.tweensInfo.Count; i++)
            {
                DrawTweensInfo(i, tg.tweensInfo[i]);
            }
        }
    }

    private void DrawCreate()
    {
        tweenList.DoLayoutList();

        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(tweensInfo.Count == 0);
        if (GUILayout.Button("  Select All  ", "miniButton"))
        {
            for (int i = 0; i < tweensInfo.Count; i++)
            {
                tweensInfo[i].isActive = true;
            }
        }
        if (GUILayout.Button("Select None", "miniButton"))
        {
            for (int i = 0; i < tweensInfo.Count; i++)
            {
                tweensInfo[i].isActive = false;
            }
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.HelpBox("Note : Undo won't work in this tab.", MessageType.Info);

        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(tweensInfo.Count == 0);
        if (GUILayout.Button("Clear"))
        {
            tweensInfo = new List<TweenInfo>();
            InitList();
        }
        EditorGUI.EndDisabledGroup();

        if (GUILayout.Button("Fetch All"))
            FindAllTweens();

        EditorGUI.BeginDisabledGroup(tweensInfo.Count == 0);
        if (GUILayout.Button("Save"))
            SaveTweenInfo();
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();
    }

    private void SaveTweenInfo()
    {
        var tempInfo = tweensInfo.Where(t => t.isActive);
        tg.tweensInfo.Add(new TweenInfoArray(tempInfo));
        tweensInfo.RemoveAll(t => tempInfo.Contains(t));
        InitList();
    }

    private void DrawTabs()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Toggle(tabIndex == 0, "Create", "miniButtonLeft"))
            tabIndex = 0;
        if (GUILayout.Toggle(tabIndex == 1, "Arrange", "miniButtonMid"))
            tabIndex = 1;
        if (GUILayout.Toggle(tabIndex == 2, "Run", "miniButtonRight"))
            tabIndex = 2;
        EditorGUILayout.EndHorizontal();
    }

    private void FindAllTweens()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("Select GameObject in hierarchy");
            return;
        }
        tweensInfo = Selection.activeGameObject.GetComponentsInChildren<TweenBase>(true).Select(t => new TweenInfo() { tween = t }).ToList();
        InitList();
    }

    private GUIStyle GetCenteredStyle()
    {
        var centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.UpperCenter;
        return centeredStyle;
    }

    private void DrawGroupElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        if (!isDisplay)
        {
            GUI.enabled = index == tweenList.count;
            return;
        }
        var element = groupList.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
    }

    private void DrawCreateElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        if (!isDisplay)
        {
            GUI.enabled = index == tweenList.count;
            return;
        }
        var tweenInfo = tweenList.list[index] as TweenInfo;

        var newRect = new Rect(rect.x, rect.y + 3, 20, EditorGUIUtility.singleLineHeight);
        tweenInfo.isActive = EditorGUI.ToggleLeft(newRect, GUIContent.none, tweenInfo.isActive);
        newRect = newRect.Add(20, 2, rect.width - 60, 0);
        tweenInfo.tween = (TweenBase)EditorGUI.ObjectField(newRect, tweenInfo.tween, typeof(TweenBase), true);

        int indexOfComp = -1;
        if (tweenInfo.tween != null)
        {
            indexOfComp = System.Array.FindIndex(tweenInfo.tween.gameObject.GetComponents<Component>(), t => t.Equals(tweenInfo.tween));
        }
        newRect = newRect.Add(newRect.width, 0, 20 - newRect.width, 0);
        EditorGUI.LabelField(newRect, indexOfComp.ToString(), GetCenteredStyle());

        newRect = new Rect(rect.x + 20, newRect.y + EditorGUIUtility.singleLineHeight + 5, 100, newRect.height);
        tweenInfo.disableAtInit = EditorGUI.ToggleLeft(newRect, "Disable At Init", tweenInfo.disableAtInit);

        newRect.x += 110;
        tweenInfo.triggerNextOnFinish = EditorGUI.ToggleLeft(newRect, "Play Next", tweenInfo.triggerNextOnFinish);
    }

}
