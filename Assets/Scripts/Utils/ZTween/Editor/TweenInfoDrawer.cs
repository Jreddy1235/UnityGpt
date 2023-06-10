using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

public class TweenInfoDrawer
{
    private ReorderableList tweenList;
    private static bool isDisplay;
    private TweenInfoArray tweenInfoArray;
    private System.Action<int> onMoveTo;

    public void Draw(TweenInfoArray tweenInfoArray, System.Action<int> onMoveTo)
    {
        if (tweenList == null)
        {
            isDisplay = EditorPrefs.GetBool("TweensFoldOut", true);
            this.tweenInfoArray = tweenInfoArray;
            InitList();
        }
        this.onMoveTo = onMoveTo;
        tweenList.DoLayoutList();
    }

    public void InitList()
    {
        tweenList = new ReorderableList(tweenInfoArray.list, typeof(TweenInfo), true, true, true, true);
        tweenList.drawElementCallback = DrawElement;
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
    }

    private GUIStyle GetCenteredStyle()
    {
        var centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.UpperCenter;
        return centeredStyle;
    }

    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        if (!isDisplay)
        {
            GUI.enabled = index == tweenList.count;
            return;
        }
        if (index >= tweenInfoArray.list.Count || index < 0)
            return;
        var tweenInfo = tweenInfoArray.list[index];

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

        newRect = new Rect(rect.x - 15, newRect.y + EditorGUIUtility.singleLineHeight + 5, 100, newRect.height);
        tweenInfo.disableAtInit = EditorGUI.ToggleLeft(newRect, "Disable At Init", tweenInfo.disableAtInit);

        newRect.x += 110;
        newRect.width = 80;
        tweenInfo.triggerNextOnFinish = EditorGUI.ToggleLeft(newRect, "Play Next", tweenInfo.triggerNextOnFinish);

        newRect.x += 80;
        newRect.width = 30;
        if (GUI.Button(newRect, "\u25B6") && onMoveTo != null)
        {
            onMoveTo.Invoke(index);
        }
    }

}
