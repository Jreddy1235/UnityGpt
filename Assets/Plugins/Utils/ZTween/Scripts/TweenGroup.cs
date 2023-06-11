using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Group tweens and play them sequentially or parallely
/// </summary>
[AddComponentMenu("ZTween/Tween Group")]
public class TweenGroup : MonoBehaviour, ITweenGroup
{
    [System.Serializable]
    public class TweenGroupEvent : UnityEvent<string>
    {
    }

    /// <summary>
    /// Key to execute methods from script
    /// </summary>
    public string key;

    /// <summary>
    /// List of tweens and their properties
    /// </summary>
    public List<TweenInfoArray> tweensInfo = new List<TweenInfoArray>();

    /// <summary>
    /// Event to call once all tweens finish executing
    /// </summary>
    public TweenGroupEvent onFinished;

    /// <summary>
    /// Does this component contains group of other TweenGroups
    /// </summary>
    public bool hasSubGroups;

    /// <summary>
    /// Group of other TweenGroups
    /// </summary>
    public List<TweenGroup> groups = new List<TweenGroup>();

    /// <summary>
    /// If true, this group will start on OnEnable
    /// </summary>
    public bool autoStart = true;

    /// <summary>
    /// Should execute next group in list once this group completes executing
    /// </summary>
    public bool autoPlayNext = true;

    /// <summary>
    /// Current tween being played
    /// </summary>
    public int currentIndex;

    /// <summary>
    /// Default group to play on autostart
    /// </summary>
    public string defaultKey;

    private bool isInit;

    /// <summary>
    /// Check for autoPlay
    /// </summary>
    void OnEnable()
    {
        Init();
        if (autoStart)
        {
            if (hasSubGroups)
                Play(defaultKey);
            else
                PlayForward();
        }
    }

    /// <summary>
    /// Resets properties of Tweens and respective gameobjects
    /// </summary>
    public void ResetAll()
    {
        if (hasSubGroups)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].ResetAll();
            }
        }
        else
        {
            for (int i = 0; i < tweensInfo.Count; i++)
            {
                Reset(i);
            }
        }
    }

    #region ITweenGroup implementation

    /// <summary>
    /// Init events and properties before starting any of the tweens
    /// </summary>
    public void Init()
    {
        currentIndex = 0;
        if (hasSubGroups)
        {
            foreach (var item in groups)
            {
                item.Init();
                if (!isInit)
                {
                    isInit = true;
                    item.onFinished.AddListener((x) =>
                    {
                        int index = groups.FindIndex(t => t.key == x);
                        OnFinished(index == groups.Count - 1);
                    });
                }
            }
        }
        else
        {
            for (int i = 0; i < tweensInfo.Count; i++)
            {
                foreach (var tweenInfo in tweensInfo[i])
                {
                    if (!tweenInfo.isActive)
                        continue;
                    if (tweenInfo.disableAtInit)
                        tweenInfo.tween.gameObject.SetActive(false);
                    tweenInfo.tween.SetCurrentValueToStart();
                    if (!tweenInfo.isInit)
                    {
                        tweenInfo.isInit = true;
                        tweenInfo.tween.AddOnFinished(() =>
                        {
                            OnFinished(tweenInfo.triggerNextOnFinish);
                        });
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets the group with specified key, To use within script
    /// </summary>
    public ITweenGroup GetGroup(string key)
    {
        if (this.key == key)
        {
            return this;
        }
        else if (hasSubGroups)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                ITweenGroup group = groups[i].GetGroup(key);
                if (group != null)
                    return group;
            }
        }
        return null;
    }

    /// <summary>
    /// Play the group with specified key
    /// </summary>
    /// <param name="forward">if trues, plays in forward direction or else reverse direction</param>
    public void Play(string key, bool forward = true)
    {
        currentIndex = groups.FindIndex(t => t.key == key);
        if (currentIndex != -1)
        {
            if (forward)
                groups[currentIndex].PlayForward();
            else
                groups[currentIndex].PlayReverse();
        }
        else if (groups.Count > 0)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].Play(key, forward);
            }
        }
    }

    /// <summary>
    /// Plays the tween in forward direction
    /// </summary>
    public void PlayForward()
    {
        if ((hasSubGroups && currentIndex >= groups.Count) || (!hasSubGroups && currentIndex >= tweensInfo.Count))
        {
            currentIndex = 0;
            Init();
        }

        if (hasSubGroups)
        {
            groups[currentIndex].PlayForward();
        }
        else
        {
            foreach (var item in tweensInfo[currentIndex])
            {
                if (!item.isActive)
                    continue;
                if (!item.tween.gameObject.activeSelf && item.disableAtInit)
                    item.tween.gameObject.SetActive(true);
                item.tween.PlayForward();
            }
        }
    }

    /// <summary>
    /// Plays the tween in reverse direction
    /// </summary>
    public void PlayReverse()
    {
        if ((hasSubGroups && currentIndex >= groups.Count) || (!hasSubGroups && currentIndex >= tweensInfo.Count))
        {
            currentIndex = 0;
            Init();
        }

        if (hasSubGroups)
        {
            groups[currentIndex].PlayReverse();
        }
        else
        {
            if (currentIndex >= tweensInfo.Count)
                currentIndex = 0;
            foreach (var item in tweensInfo[currentIndex])
            {
                if (!item.isActive)
                    continue;
                if (!item.tween.gameObject.activeSelf && item.disableAtInit)
                    item.tween.gameObject.SetActive(true);
                item.tween.PlayReverse();
            }
        }
    }

    /// <summary>
    /// Raises the finished event for this group
    /// </summary>
    public void OnFinished(bool isLast)
    {
        if (!hasSubGroups && isLast)
        {
            if (autoPlayNext)
            {
                currentIndex++;
                if (currentIndex >= tweensInfo.Count)
                    onFinished.Invoke(key);
                else
                    PlayForward();
            }
        }
        else if (hasSubGroups)
        {
            if (autoPlayNext)
            {
                currentIndex++;
                if (currentIndex >= groups.Count)
                    onFinished.Invoke(key);
                else
                    PlayForward();
            }
        }
    }

    /// <summary>
    /// Reset the group with specified key.
    /// </summary>
    public void Reset(string key)
    {
        if (this.key == key)
        {
            ResetAll();
        }
        else if (hasSubGroups)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].Reset(key);
            }
        }
    }

    /// <summary>
    /// Reset the properties of tween at specified index.
    /// </summary>
    public void Reset(int index)
    {
        if (!hasSubGroups && index < tweensInfo.Count)
        {
            foreach (var item in tweensInfo[index])
            {
                if (!item.isActive)
                    continue;
                if (item.disableAtInit)
                    item.tween.gameObject.SetActive(false);
                item.tween.SetCurrentValueToStart();
            }
        }
    }

    #endregion
}
