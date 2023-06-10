using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// Tween properties for each tween
/// </summary>
[System.Serializable]
public class TweenInfo
{
    [HideInInspector] public bool isInit = false;
    public bool isActive = true;
    public TweenBase tween;
    public bool disableAtInit;
    public bool triggerNextOnFinish;
}

/// <summary>
/// To serialize array of tween info in list
/// </summary>
[System.Serializable]
public class TweenInfoArray : IEnumerable<TweenInfo>
{
    public TweenInfoArray(IEnumerable<TweenInfo> array)
    {
        this.list = array.ToList();
    }

    public List<TweenInfo> list;

    public TweenInfo this[int index]
    {
        get
        {
            return list[index];
        }
        set
        {
            list[index] = value;
        }
    }

    public int Count
    {
        get
        {
            return list.Count;
        }
    }

    #region IEnumerable implementation

    public IEnumerator<TweenInfo> GetEnumerator()
    {
        for (int i = 0; i < Count; ++i)
            yield return list[i];
    }

    #endregion

    #region IEnumerable implementation

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}
