using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWithWindow : MonoBehaviour {

    public bool enable = true;
    [Space]
    public SmokeManager smokeManager;
    [Range(0f, 1f)]
    public float wallAboveWindow = 0.1f;
    [Range(0f, 1f)]
    public float wallBelowWindow = 0f;
    [Range(0f, 1f)]
    public float wallLeftToWindow = 0.35f;
    [Range(0f, 1f)]
    public float wallRightToWindow = 0.35f;
    public int wallThickness = 3;
    private Vector3Int gridPos;
    private Vector3Int size;

    private Vector3Int bSize;

    int id(int i, int j, int k)
    {
        return (i + this.bSize.x * j + this.bSize.x * this.bSize.y * k);
    }

    void OnDrawGizmosSelected()
    {
        if (!enable || smokeManager == null) return;

        Vector3 gizmoSize = new Vector3(wallThickness, smokeManager.size.y, smokeManager.size.z) / smokeManager.gridScale;
        Vector3Int gridPos = new Vector3Int(smokeManager.worldToGridPos(transform.position).x, smokeManager.size.y / 2, smokeManager.size.z / 2);
        Vector3 windowSize = new Vector3(wallThickness, (int)((1f - wallAboveWindow - wallBelowWindow) * smokeManager.size.y),
                                        (int)((1f - wallLeftToWindow - wallRightToWindow) * smokeManager.size.z)) / smokeManager.gridScale;
        Vector3Int windowPos = gridPos + new Vector3Int(0, Mathf.RoundToInt((wallAboveWindow/2f - wallBelowWindow/2f) * smokeManager.size.y),
                                                        Mathf.RoundToInt((wallRightToWindow/2f - wallLeftToWindow/2f) * smokeManager.size.z));
        Gizmos.DrawWireCube(smokeManager.gridToWorldPos(gridPos, true), gizmoSize);
        Gizmos.DrawWireCube(smokeManager.gridToWorldPos(windowPos, true), windowSize);
    }

    private void Awake()
    {
        size = smokeManager.size - new Vector3Int(2, 2, 2);
        smokeManager.fluidBoundries.AddBoundriesCondition(checkBoundry);
        smokeManager.fluidBoundries.AddBoundriesSetter(wallBoundry);
    }

    private void Update()
    {
        if (!enable) return;

        gridPos = smokeManager.worldToGridPos(transform.position);
    }

    public bool checkBoundry(int x, int y, int z)
    {
        return (x <= Mathf.CeilToInt(gridPos.x - (wallThickness / 2)) || x >= Mathf.CeilToInt(gridPos.x + (wallThickness / 2)) ||
                !(y <= size.y * wallAboveWindow || y >= size.y * (1f - wallBelowWindow) ||
                  z <= size.z * wallLeftToWindow || z >= size.z * (1f - wallRightToWindow)));
    }

    public void wallBoundry(int b, float[] x)
    {
        if (!enable) return;

        bSize = size + new Vector3Int(2,2,2);
            
        for (int i = 1; i <= size.y; i++)
        {
            for (int k = 1; k <= size.z; k++)
            {
                if(i > size.y * wallAboveWindow && i < size.y * (1f - wallBelowWindow) &&
                   k > size.z * wallLeftToWindow && k < size.z * (1f - wallRightToWindow))
                {
                    k = Mathf.CeilToInt(size.z * (1f - wallRightToWindow));
                }

                // middle wall
                x[id(gridPos.x + (wallThickness / 2), i, k)] = b == 1 ? -x[id(gridPos.x + (wallThickness / 2) + 1, i, k)] :
                                                                        x[id(gridPos.x + (wallThickness / 2) + 1, i, k)];
                x[id(gridPos.x + (wallThickness / 2) - 1, i, k)] = 0;
                x[id(gridPos.x - (wallThickness / 2), i, k)] = b == 1 ? -x[id(gridPos.x - (wallThickness / 2) - 1, i, k)] :
                                                                        x[id(gridPos.x - (wallThickness / 2) - 1, i, k)];
                x[id(gridPos.x - (wallThickness / 2) + 1, i, k)] = 0;
            }
        }

        for (int i = Mathf.CeilToInt(size.z * wallLeftToWindow); i <= size.z * (1f - wallRightToWindow); i++)
        {
            for (int k = Mathf.CeilToInt(gridPos.x - (wallThickness / 2)); k < (gridPos.x + (wallThickness / 2)); k++)
            {
                // upper window frame
                x[id(k, (int)(size.y * wallAboveWindow), i)] = b == 2 ? -x[id(k, (int)(size.y * wallAboveWindow) + 1, i)] :
                                                                            x[id(k, (int)(size.y * wallAboveWindow) + 1, i)];
                x[id(k, (int)(size.y * wallAboveWindow) - 1, i)] = 0;
            }
        }
            
        for (int i = (int)(size.y * wallAboveWindow) + 1; i < size.y * (1f - wallBelowWindow); i++)
        {
            for (int k = Mathf.CeilToInt(gridPos.x - (wallThickness / 2)); k < (gridPos.x + (wallThickness / 2)); k++)
            {
                // side window frames
                // right
                x[id(k, i, (int)(size.z * wallLeftToWindow))] = b == 3 ? -x[id(k, i, (int)(size.z * wallLeftToWindow) + 1)] :
                                                                            x[id(k, i, (int)(size.z * wallLeftToWindow) + 1)];
                x[id(k, i, (int)(size.z * wallLeftToWindow) - 1)] = 0;

                // left
                x[id(k, i, (int)(size.z * (1f - wallRightToWindow)))] = b == 3 ? -x[id(k, i, (int)(size.z * (1f - wallRightToWindow)) - 1)] :
                                                                                    x[id(k, i, (int)(size.z * (1f - wallRightToWindow)) - 1)];
                x[id(k, i, (int)(size.z * (1f - wallRightToWindow)) + 1)] = 0;
            }
        }
    }
}
