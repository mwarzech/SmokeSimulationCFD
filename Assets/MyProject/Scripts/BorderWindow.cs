using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Window {
    public Vector2Int size;
    public Vector3Int topLeft;
    public Vector3Int topRight;
    public Vector3Int bottomLeft;
    public Vector3Int bottomRight;
    public float wallAboveWindow = 0.35f;
    public float wallBelowWindow = 0.35f;
    public float wallLeftToWindow = 0.35f;
    public float wallRightToWindow = 0.35f;

    protected Window(Vector4 windowBorders)
    {
        wallAboveWindow = windowBorders.x;
        wallBelowWindow = windowBorders.y;
        wallLeftToWindow = windowBorders.z;
        wallRightToWindow = windowBorders.w;
    }

    public abstract void UpdateWindow(Vector3Int size, Vector4 windowBorders);
}

public class WindowLeft : Window
{
    public WindowLeft(Vector3Int size, Vector4 windowBorders) : base(windowBorders)
    {
        this.size = new Vector2Int(size.z, size.y);
        topLeft = new Vector3Int(0, (int)(wallAboveWindow * size.y), (int)(wallLeftToWindow * size.z));
        topRight = new Vector3Int(0, (int)(wallAboveWindow * size.y), (int)((1f - wallRightToWindow) * size.z));
        bottomLeft = new Vector3Int(0, (int)((1f - wallBelowWindow) * size.y), (int)(wallLeftToWindow * size.z));
        bottomRight = new Vector3Int(0, (int)((1f - wallBelowWindow) * size.y), (int)((1f - wallRightToWindow) * size.z));
    }

    public override void UpdateWindow(Vector3Int size, Vector4 windowBorders)
    {
        wallAboveWindow = windowBorders.x;
        wallBelowWindow = windowBorders.y;
        wallLeftToWindow = windowBorders.z;
        wallRightToWindow = windowBorders.w;
        topLeft = new Vector3Int(0, (int)(wallAboveWindow * size.y), (int)(wallLeftToWindow * size.z));
        topRight = new Vector3Int(0, (int)(wallAboveWindow * size.y), (int)((1f - wallRightToWindow) * size.z));
        bottomLeft = new Vector3Int(0, (int)((1f - wallBelowWindow) * size.y), (int)(wallLeftToWindow * size.z));
        bottomRight = new Vector3Int(0, (int)((1f - wallBelowWindow) * size.y), (int)((1f - wallRightToWindow) * size.z));
    }
}

public class WindowRight : Window
{
    public WindowRight(Vector3Int size, Vector4 windowBorders) : base(windowBorders)
    {
        this.size = new Vector2Int(size.z, size.y);
        topLeft = new Vector3Int(size.x, (int)(wallAboveWindow * size.y), (int)(wallLeftToWindow * size.z));
        topRight = new Vector3Int(size.x, (int)(wallAboveWindow * size.y), (int)((1f - wallRightToWindow) * size.z));
        bottomLeft = new Vector3Int(size.x, (int)((1f - wallBelowWindow) * size.y), (int)(wallLeftToWindow * size.z));
        bottomRight = new Vector3Int(size.x, (int)((1f - wallBelowWindow) * size.y), (int)((1f - wallRightToWindow) * size.z));
    }

    public override void UpdateWindow(Vector3Int size, Vector4 windowBorders)
    {
        wallAboveWindow = windowBorders.x;
        wallBelowWindow = windowBorders.y;
        wallLeftToWindow = windowBorders.z;
        wallRightToWindow = windowBorders.w;
        topLeft = new Vector3Int(size.x, (int)(wallAboveWindow * size.y), (int)(wallLeftToWindow * size.z));
        topRight = new Vector3Int(size.x, (int)(wallAboveWindow * size.y), (int)((1f - wallRightToWindow) * size.z));
        bottomLeft = new Vector3Int(size.x, (int)((1f - wallBelowWindow) * size.y), (int)(wallLeftToWindow * size.z));
        bottomRight = new Vector3Int(size.x, (int)((1f - wallBelowWindow) * size.y), (int)((1f - wallRightToWindow) * size.z));
    }
}

public class WindowFront : Window
{
    public WindowFront(Vector3Int size, Vector4 windowBorders) : base(windowBorders)
    {
        this.size = new Vector2Int(size.x, size.y);
        topLeft = new Vector3Int((int)(wallLeftToWindow * size.x), (int)(wallAboveWindow * size.y), size.z);
        topRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), (int)(wallAboveWindow * size.y), size.z);
        bottomLeft = new Vector3Int((int)(wallLeftToWindow * size.x), (int)((1f - wallBelowWindow) * size.y), size.z);
        bottomRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), (int)((1f - wallBelowWindow) * size.y), size.z);
    }

    public override void UpdateWindow(Vector3Int size, Vector4 windowBorders)
    {
        wallAboveWindow = windowBorders.x;
        wallBelowWindow = windowBorders.y;
        wallLeftToWindow = windowBorders.z;
        wallRightToWindow = windowBorders.w;
        topLeft = new Vector3Int((int)(wallLeftToWindow * size.x), (int)(wallAboveWindow * size.y), size.z);
        topRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), (int)(wallAboveWindow * size.y), size.z);
        bottomLeft = new Vector3Int((int)(wallLeftToWindow * size.x), (int)((1f - wallBelowWindow) * size.y), size.z);
        bottomRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), (int)((1f - wallBelowWindow) * size.y), size.z);
    }
}

public class WindowBack : Window
{
    public WindowBack(Vector3Int size, Vector4 windowBorders) : base(windowBorders)
    {
        this.size = new Vector2Int(size.x, size.y);
        topLeft = new Vector3Int((int)(wallLeftToWindow * size.x), (int)(wallAboveWindow * size.y), 0);
        topRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), (int)(wallAboveWindow * size.y), 0);
        bottomLeft = new Vector3Int((int)(wallLeftToWindow * size.x), (int)((1f - wallBelowWindow) * size.y), 0);
        bottomRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), (int)((1f - wallBelowWindow) * size.y), 0);
    }

    public override void UpdateWindow(Vector3Int size, Vector4 windowBorders)
    {
        wallAboveWindow = windowBorders.x;
        wallBelowWindow = windowBorders.y;
        wallLeftToWindow = windowBorders.z;
        wallRightToWindow = windowBorders.w;
        topLeft = new Vector3Int((int)(wallLeftToWindow * size.x), (int)(wallAboveWindow * size.y), 0);
        topRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), (int)(wallAboveWindow * size.y), 0);
        bottomLeft = new Vector3Int((int)(wallLeftToWindow * size.x), (int)((1f - wallBelowWindow) * size.y), 0);
        bottomRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), (int)((1f - wallBelowWindow) * size.y), 0);
    }
}

public class WindowTop : Window
{
    public WindowTop(Vector3Int size, Vector4 windowBorders) : base(windowBorders)
    {
        this.size = new Vector2Int(size.x, size.z);
        topLeft = new Vector3Int((int)(wallLeftToWindow * size.x), 0, (int)(wallAboveWindow * size.z));
        topRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), 0, (int)(wallAboveWindow * size.z));
        bottomLeft = new Vector3Int((int)(wallLeftToWindow * size.x), 0, (int)((1f - wallBelowWindow) * size.z));
        bottomRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), 0, (int)((1f - wallBelowWindow) * size.z));
    }

    public override void UpdateWindow(Vector3Int size, Vector4 windowBorders)
    {
        wallAboveWindow = windowBorders.x;
        wallBelowWindow = windowBorders.y;
        wallLeftToWindow = windowBorders.z;
        wallRightToWindow = windowBorders.w;
        topLeft = new Vector3Int((int)(wallLeftToWindow * size.x), 0, (int)(wallAboveWindow * size.z));
        topRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), 0, (int)(wallAboveWindow * size.z));
        bottomLeft = new Vector3Int((int)(wallLeftToWindow * size.x), 0, (int)((1f - wallBelowWindow) * size.z));
        bottomRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), 0, (int)((1f - wallBelowWindow) * size.z));
    }
}

public class WindowBottom : Window
{
    public WindowBottom(Vector3Int size, Vector4 windowBorders) : base(windowBorders)
    {
        this.size = new Vector2Int(size.x, size.z);
        topLeft = new Vector3Int((int)(wallLeftToWindow * size.x), size.y, (int)(wallAboveWindow * size.z));
        topRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), size.y, (int)(wallAboveWindow * size.z));
        bottomLeft = new Vector3Int((int)(wallLeftToWindow * size.x), size.y, (int)((1f - wallBelowWindow) * size.z));
        bottomRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), size.y, (int)((1f - wallBelowWindow) * size.z));
    }

    public override void UpdateWindow(Vector3Int size, Vector4 windowBorders)
    {
        wallAboveWindow = windowBorders.x;
        wallBelowWindow = windowBorders.y;
        wallLeftToWindow = windowBorders.z;
        wallRightToWindow = windowBorders.w;
        topLeft = new Vector3Int((int)(wallLeftToWindow * size.x), size.y, (int)(wallAboveWindow * size.z));
        topRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), size.y, (int)(wallAboveWindow * size.z));
        bottomLeft = new Vector3Int((int)(wallLeftToWindow * size.x), size.y, (int)((1f - wallBelowWindow) * size.z));
        bottomRight = new Vector3Int((int)((1f - wallRightToWindow) * size.x), size.y, (int)((1f - wallBelowWindow) * size.z));
    }
}


public class BorderWindow : MonoBehaviour {

    public SmokeManager smokeManager;
    public bool enable;
    public Side windowSide;
    [Range(0f, 0.5f)]
    public float wallAboveWindow = 0.35f;
    [Range(0f, 0.5f)]
    public float wallBelowWindow = 0.35f;
    [Range(0f, 0.5f)]
    public float wallLeftToWindow = 0.35f;
    [Range(0f, 0.5f)]
    public float wallRightToWindow = 0.35f;

    private Window window;
    
    void OnDrawGizmosSelected()
    {
        if (enable && smokeManager != null)
        {
            SetWindow();
            Gizmos.DrawLine(smokeManager.gridToWorldPos(window.topLeft, true), smokeManager.gridToWorldPos(window.topRight, true));
            Gizmos.DrawLine(smokeManager.gridToWorldPos(window.topLeft, true), smokeManager.gridToWorldPos(window.bottomLeft, true));
            Gizmos.DrawLine(smokeManager.gridToWorldPos(window.topRight, true), smokeManager.gridToWorldPos(window.bottomRight, true));
            Gizmos.DrawLine(smokeManager.gridToWorldPos(window.bottomLeft, true), smokeManager.gridToWorldPos(window.bottomRight, true));
        }
    }

    private void Awake()
    {
        SetWindow();
        smokeManager.fluidBoundries.AddWindowCondition(CheckIfWindow);
    }

    private void UpdateWindow()
    {
        Vector3Int size = smokeManager.size - new Vector3Int(2, 2, 2);
        Vector4 windowBorders = new Vector4(wallAboveWindow, wallBelowWindow, wallLeftToWindow, wallRightToWindow);
        window.UpdateWindow(size, windowBorders);
    }

    private void SetWindow()
    {
        Vector3Int size = smokeManager.size - new Vector3Int(2, 2, 2);
        Vector4 windowBorders = new Vector4(wallAboveWindow, wallBelowWindow, wallLeftToWindow, wallRightToWindow);
        switch (windowSide)
        {
            case Side.front:
                window = new WindowFront(size, windowBorders);
                break;
            case Side.back:
                window = new WindowBack(size, windowBorders);
                break;
            case Side.left:
                window = new WindowLeft(size, windowBorders);
                break;
            case Side.right:
                window = new WindowRight(size, windowBorders);
                break;
            case Side.top:
                window = new WindowTop(size, windowBorders);
                break;
            case Side.bottom:
                window = new WindowBottom(size, windowBorders);
                break;
        }
    }

    public bool CheckIfWindow(Side side, int i, int j)
    {
        if (!enable || side != this.windowSide) return false;
        return (i >= window.size.y * wallAboveWindow && i <= window.size.y * (1f - wallBelowWindow) &&
                j >= window.size.x * wallLeftToWindow && j <= window.size.x * (1f - wallRightToWindow));
    }
}
