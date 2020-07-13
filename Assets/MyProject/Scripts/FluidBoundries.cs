using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side { front = 0, back = 1, left = 2, right = 3, top = 4, bottom = 5 };

public class FluidBoundries : MonoBehaviour
{
    public delegate bool BoundriesConditionMethod(int x, int y, int z);
    public delegate bool BorderWindowConditionMethod(Side side, int i, int j);
    public delegate void BoundriesSetterMethod(int b, float[] x);

    private List<BoundriesConditionMethod> conditionList = new List<BoundriesConditionMethod>();
    private List<BorderWindowConditionMethod> windowList = new List<BorderWindowConditionMethod>();
    private List<BoundriesSetterMethod> actionList = new List<BoundriesSetterMethod>();

    public void AddWindowCondition(BorderWindowConditionMethod method)
    {
        windowList.Add(method);
    }

    public void RemoveWindowCondition(BorderWindowConditionMethod method)
    {
        windowList.Remove(method);
    }

    public void AddBoundriesCondition(BoundriesConditionMethod method)
    {
        conditionList.Add(method);
    }

    public void RemoveBoundriesCondition(BoundriesConditionMethod method)
    {
        conditionList.Remove(method);
    }

    public void AddBoundriesSetter(BoundriesSetterMethod method)
    {
        actionList.Add(method);
    }

    public void RemoveBoundriesSetter(BoundriesSetterMethod method)
    {
        actionList.Remove(method);
    }

    public bool CheckAndConditions(int x, int y, int z)
    {
        if (conditionList == null || conditionList.Count <= 0) return false;
        foreach(BoundriesConditionMethod condition in conditionList)
        {
            if (!condition.Invoke(x, y, z)) return false;
        }
        return true;
    }

    public bool CheckOrConditions(int x, int y, int z)
    {
        if (conditionList == null || conditionList.Count <= 0) return false;
        foreach (BoundriesConditionMethod condition in conditionList)
        {
            if (condition.Invoke(x, y, z)) return true;
        }
        return false;
    }

    public bool CheckBorderIfWindow(Side side, int i, int j)
    {
        if (windowList == null || windowList.Count <= 0) return false;
        foreach (BorderWindowConditionMethod window in windowList)
        {
            if (window.Invoke(side, i, j)) return true;
        }
        return false;
    }

    public void SetBoundriesAction(int b, float[] x)
    {
        foreach (BoundriesSetterMethod action in actionList)
        {
            action.Invoke(b, x);
        }
    }
}
