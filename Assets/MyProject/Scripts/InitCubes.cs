using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitCubes : MonoBehaviour {

    readonly Vector3Int gridSize = new Vector3Int(32, 32, 32);
    public Transform center;
    public float scale = 10;

    public Material[] materials;
    public GameObject prefab;

    void CreateCubes()
    {
        Vector3 startPos = center.position - (Vector3.one * (scale / 2f));
        Vector3 voxelScale = new Vector3(scale / (float)gridSize.x, scale / (float)gridSize.y, scale / (float)gridSize.z);
        for (int x = 0; x < gridSize.x; ++x)
        {
            for (int y = 0; y < gridSize.y; ++y)
            {
                for (int z = 0; z < gridSize.z; ++z)
                {
                    Vector3 relPos = new Vector3(x * voxelScale.x, y * voxelScale.y, z * voxelScale.z);
                    GameObject voxel = Instantiate(prefab, startPos + relPos, center.rotation, transform);
                    voxel.transform.localScale = voxelScale;
                }
            }
        }
    }

	void Start () {
        CreateCubes();
	}
}
