using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeEmmiter : MonoBehaviour {

    public bool enable = true;
    [Space]
    public SmokeManager smokeManager;
    public float strength = 1;
    public Vector3Int size = Vector3Int.one;
    private Vector3Int gridPos;

    void OnDrawGizmosSelected()
    {
        if(enable && smokeManager != null)
        {
            Vector3 gizmoSize = ((Vector3)size) / smokeManager.gridScale;
            Vector3Int gridPos = smokeManager.worldToGridPos(transform.position);
            Gizmos.DrawWireCube(smokeManager.gridToWorldPos(gridPos, true), gizmoSize);
        }
    }

    private void Awake()
    {
        smokeManager.onSmokeUpdate.AddOnSmokeUpdateMethod(EmmitSmoke);
    }

    private void Update()
    {
        gridPos = smokeManager.worldToGridPos(transform.position) - new Vector3Int(size.x / 2, size.y / 2, size.z / 2);
    }

    public void EmmitSmoke()
    {
        if (enable)
        {
            for(int x = 0; x < size.x; ++x)
            {
                for(int y = 0; y < size.y; ++y)
                {
                    for(int z = 0; z < size.z; ++z)
                    {
                        if (smokeManager.isInsideGrid(gridPos + new Vector3Int(x, y, z)))
                        {
                            smokeManager.setDensityAtPoint(gridPos + new Vector3Int(x, y, z), strength);
                        }
                    }
                }
            }
        }
    }
}
