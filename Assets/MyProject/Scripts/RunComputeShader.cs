using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunComputeShader : MonoBehaviour {

    public ComputeShader simulationShader;
    public Material material;
    public Camera cam;
    public Transform obj;

    int part1,part2,part3,part4,part5;

    static readonly Vector3Int gridSize = new Vector3Int(32, 32, 32);
    static readonly float gridScale = (gridSize.x / 20.0f);
    static readonly Vector3Int gridThreadCount = new Vector3Int(8, 8, 8);
    static readonly int voxelCount = gridSize.x * gridSize.y * gridSize.z;

    public struct Voxel
    {
        public float d;
        public Vector3 v;
        public float curl;
        public float temp;
    };

    const int voxelSize = 24;
    public Voxel[] voxels = new Voxel[voxelCount];

    RenderTexture tex3d;

    private ComputeBuffer[] dataBuffers = new ComputeBuffer[2];
    private int currentBuffer = 0;
    private ComputeBuffer ReadBuffer
    {
        get
        {
            if(dataBuffers[currentBuffer%2] == null)
            {
                dataBuffers[currentBuffer % 2] = new ComputeBuffer(voxelCount, voxelSize, ComputeBufferType.Default);
            }
            return dataBuffers[currentBuffer % 2];
        }
        set
        {
            dataBuffers[currentBuffer % 2] = value;
        }
    }
    private ComputeBuffer WriteBuffer
    {
        get
        {
            if (dataBuffers[(currentBuffer + 1) % 2] == null)
            {
                dataBuffers[(currentBuffer + 1) % 2] = new ComputeBuffer(voxelCount, voxelSize, ComputeBufferType.Default);
            }
            return dataBuffers[(currentBuffer + 1) % 2];
        }
        set
        {
            dataBuffers[(currentBuffer + 1) % 2] = value;
        }
    }

    void SetupShaders()
    {
        part1 = simulationShader.FindKernel("Part1");
        part2 = simulationShader.FindKernel("Part2");
        part3 = simulationShader.FindKernel("Part3");
        part4 = simulationShader.FindKernel("Part4");
        part5 = simulationShader.FindKernel("Part5");

        tex3d = new RenderTexture(gridSize.x, gridSize.y, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Default);
        tex3d.enableRandomWrite = true;
        tex3d.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
        tex3d.volumeDepth = gridSize.z;
        tex3d.Create();

        simulationShader.SetTexture(part5, "Result", tex3d);
    }

    void SetBuffers(int kernel)
    {
        simulationShader.SetBuffer(kernel, "oldD", ReadBuffer);
        simulationShader.SetBuffer(kernel, "newD", WriteBuffer);
    }

    void DispatchShader(int kernel)
    {
        simulationShader.Dispatch(kernel, gridSize.x / gridThreadCount.x, gridSize.y / gridThreadCount.y, gridSize.z / gridThreadCount.z);
    }

    void HandleDispatchShader(int kernel)
    {
        SetBuffers(kernel);
        DispatchShader(kernel);
        SwapBuffers();
    }

    void SwapBuffers()
    {
        currentBuffer = (currentBuffer + 1) % 2;
    }

    void DispatchAll()
    {
        HandleDispatchShader(part1);
        HandleDispatchShader(part2);
        HandleDispatchShader(part3);
        HandleDispatchShader(part4);
        HandleDispatchShader(part5);
    }

    private int cI(int x, int y, int z)
    {
        return x + gridSize.x * y + gridSize.x * gridSize.y * z;
    }


    float avgTemp()
    {
        float Tamb = 0;

        Voxel[] temp = new Voxel[gridSize.x * gridSize.y * gridSize.z];
        WriteBuffer.GetData(temp);

        // sum all temperatures
        for (int i = 1; i <= gridSize.x - 2; ++i)
        {
            for (int j = 1; j <= gridSize.y - 2; ++j)
            {
                for (int k = 1; k <= gridSize.z - 2; ++k)
                {
                    Tamb += temp[cI(i, j, k)].d;
                }
            }
        }

        // get average temperature
        Tamb /= ((gridSize.x - 2) * (gridSize.y - 2) * (gridSize.z - 2));
        Debug.Log(Tamb);

        return Tamb;
    }

    private void Start()
    {
        SetupShaders();
        material.SetVector("_GridSize", new Vector4(gridSize.x, gridSize.y, gridSize.z, 0));
        material.SetFloat("_GridScale", gridScale);
        material.SetFloat("_GridRadius", (Mathf.Sqrt((gridSize.x * gridSize.x + gridSize.y * gridSize.y + gridSize.z * gridSize.z) / 4f) / gridScale));
        material.SetTexture("_DensTex", tex3d);

        for (int i = 0; i < gridSize.x; ++i)
        {
            for (int j = 0; j < gridSize.y; ++j)
            {
                for (int k = 0; k < gridSize.z; ++k)
                {
                    voxels[cI(i, j, k)].v.x = 0.0f;
                    voxels[cI(i, j, k)].v.y = 0.0f;
                    voxels[cI(i, j, k)].v.z = 0.0f;
                    voxels[cI(i, j, k)].d = 0.0f;
                    voxels[cI(i, j, k)].temp = 0.0f;
                    voxels[cI(i, j, k)].curl = 0.0f;

                    //this.u[i][j][k] = this.uOld[i][j][k] = 0.0f;
                    //this.v[i][j][k] = this.vOld[i][j][k] = 0.0f;
                    //this.w[i][j][k] = this.wOld[i][j][k] = 0.0f;
                    //this.d[i][j][k] = this.dOld[i][j][k] = this.curl[i][j][k] = 0.0f;
                    //this.temp[i][j][k] = 0.0f;
                }
            }
        }
        //this.d[this.bSize / 2][this.bSize - 2][this.bSize / 2] = 100;
        //this.d[this.bSize / 2][1][this.bSize / 2] = 100;

        voxels[cI(gridSize.x / 2, gridSize.y - 2, gridSize.z / 2)].d = 1000;
        voxels[cI(gridSize.x / 2, 1, gridSize.z / 2)].d = 1000;

        ReadBuffer.SetData(voxels);

        /* for(int i=0;i<2;++i)
         {
             simulationShader.SetFloat("Tamb", avgTemp());
             DispatchAll();
         }*/
        StartCoroutine(SmokeEnumerator());
    }

    private void Update()
    {
        material.SetMatrix("_WorldToObject", obj.worldToLocalMatrix);
        /*simulationShader.SetFloat("Tamb", avgTemp());
        DispatchAll();
        SwapBuffers();*/
    }

    void PrintArray()
    {
        string log = "";
        Voxel[] array = new Voxel[gridSize.x * gridSize.y * gridSize.z];
        WriteBuffer.GetData(array);
        foreach (Voxel x in array)
        {
            log += x.v.z + " ";
        }
        Debug.Log(log);
    }

    IEnumerator SmokeEnumerator()
    {
        while(true)
        {
            yield return null;//new WaitForSeconds(0.2f);
            simulationShader.SetFloat("Tamb", avgTemp());
            DispatchAll();
            SwapBuffers();
            //PrintArray();
        }
    }
}