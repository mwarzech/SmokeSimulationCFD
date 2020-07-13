using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading;


public class OnSmokeUpdate
{
    public delegate void OnSmokeUpdateMethod();
    private List<OnSmokeUpdateMethod> onSmokeUpdateList = new List<OnSmokeUpdateMethod>();

    public void Invoke()
    {
        foreach(OnSmokeUpdateMethod method in onSmokeUpdateList)
        {
            method.Invoke();
        }
    }

    public void AddOnSmokeUpdateMethod(OnSmokeUpdateMethod method)
    {
        onSmokeUpdateList.Add(method);
    }

    public void RemoveOnSmokeUpdateMethod(OnSmokeUpdateMethod method)
    {
        onSmokeUpdateList.Remove(method);
    }
}

public class SmokeManager : MonoBehaviour {

    // solver variables
    public Vector3Int size = new Vector3Int(32, 32, 32);
    float dt = 0.5f;
    FluidSolver fs = new FluidSolver();
    public float scale = 20f;
    public float gridScale
    {
        get { return ((float)(size.x) / scale); }
    }
    int threadCount = 8;
    public ComputeShader shader;
    int kernel;
    ComputeBuffer buffer;
    public Material material;
    RenderTexture tex3d;
    public Transform obj;

    public OnSmokeUpdate onSmokeUpdate = new OnSmokeUpdate();
    public FluidBoundries fluidBoundries;
    public float simulationTime = 0.0f;

    private Thread calcThread;

    void SetupShader()
    {
        kernel = shader.FindKernel("CSMain");

        tex3d = new RenderTexture(size.x, size.y, 0);//, RenderTextureFormat.RFloat, RenderTextureReadWrite.Default);
        tex3d.enableRandomWrite = true;
        tex3d.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
        tex3d.volumeDepth = size.z;
        tex3d.Create();
        Vector3 gridSize = new Vector4(size.x, size.y, size.z, 0);
        material.SetVector("_GridSize", gridSize);
        material.SetFloat("_GridScale", gridScale);
        material.SetFloat("_GridRadius", Mathf.Sqrt((size.x * size.x + size.y * size.y + size.z * size.z)/4f) / gridScale);
        material.SetTexture("_DensTex", tex3d);

        buffer = new ComputeBuffer(fs.bSize.x * fs.bSize.y * fs.bSize.z, 4);

        shader.SetVector("gridSize", gridSize);
        shader.SetTexture(kernel, "Result", tex3d);
    }

    void SetBuffer()
    {
        buffer.SetData(fs.d);
        shader.SetBuffer(kernel, "Source", buffer);
    }

    void DispatchShader()
    {
        shader.Dispatch(kernel, fs.bSize.x / threadCount, fs.bSize.y / threadCount, fs.bSize.z / threadCount);
    }

    // Constructor
    public void Setup()
    {
        this.fs.setup(this.size - new Vector3Int(2,2,2), this.dt);
        this.fs.fluidBoundries = this.fluidBoundries;
        this.CalculateSmoke();
    }

    private void Start()
    {
        Setup();
        SetupShader();
        calcThread = new Thread(CalculateSmoke);
        calcThread.Start();
    }

    private void Update()
    {
        material.SetMatrix("_WorldToObject", obj.worldToLocalMatrix);
        if (calcThread.ThreadState != ThreadState.Running)
        {
            Display();
            onSmokeUpdate.Invoke();
            calcThread = new Thread(CalculateSmoke);
            calcThread.Start();
        }
    }

    public void CalculateSmoke()
    {
        this.fs.velocitySolver();
        this.fs.densitySolver();

        simulationTime += fs.dt;
        Debug.Log("Time: " + this.simulationTime);
    }

    public void Display()
    {
        SetBuffer();
        DispatchShader();
    }

    public void addDensityAtPoint(Vector3Int position, float dens)
    {
        fs.d[fs.id(position)] += dens;
    }

    public void setDensityAtPoint(Vector3Int position, float dens)
    {
        fs.d[fs.id(position)] = dens;
    }

    public void setVelocityAtPoint(Vector3Int position, Vector3 vel)
    {
        fs.u[fs.id(position)] = vel.x;
        fs.v[fs.id(position)] = vel.y;
        fs.w[fs.id(position)] = vel.z;
    }

    public Vector3Int worldToGridPos(Vector3 pos)
    {
        Vector3 localPos = this.obj.transform.worldToLocalMatrix.MultiplyPoint(pos);
        localPos *= this.gridScale;
        localPos += ((Vector3)this.size * 0.5f);
        return new Vector3Int((int)localPos.x, (int)localPos.y, (int)localPos.z);
    }

    public Vector3 gridToWorldPos(Vector3Int pos, bool inCenter = false)
    {
        Vector3 localPos = pos;
        localPos -= ((Vector3)this.size * 0.5f);
        if (inCenter) localPos += new Vector3(0.5f, 0.5f, 0.5f);
         localPos /= this.gridScale;
        return this.obj.transform.localToWorldMatrix.MultiplyPoint(localPos);
    }

    public bool isInsideGrid(Vector3Int gridPos)
    {
        return (gridPos.x >= 0 && gridPos.x < this.size.x &&
           gridPos.y >= 0 && gridPos.y < this.size.y &&
           gridPos.z >= 0 && gridPos.z < this.size.z);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(obj.position, new Vector3(size.x / gridScale, size.y / gridScale, size.z / gridScale));
    }
}
