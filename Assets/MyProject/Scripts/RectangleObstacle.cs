using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Obstacle
{
    public float height = 0.4f;
    public float depth = 0.2f;
    public Vector3Int obstaclePos;
    public Vector3Int obstacleSize;

    protected Vector3Int size;
    protected int id(int i, int j, int k)
    {
        return (i + this.size.x * j + this.size.x * this.size.y * k);
    }
    public Obstacle(float height, float depth, Vector3Int size)
    {
        this.height = height;
        this.depth = depth;
        this.size = size;
    }
    public abstract bool checkBoundry(Vector3Int pos);
    public abstract void setBoundry(int b, float[] x);
}

class ObstacleFront : Obstacle
{
    public ObstacleFront(float height, float depth, Vector3Int size) : base(height, depth, size)
    {
        obstaclePos = new Vector3Int(size.x / 2, size.y - ((int)(size.y * height) / 2), size.z - ((int)(size.z * depth) / 2));
        obstacleSize = new Vector3Int(size.x, (int)(size.y * height), (int)(size.z * depth));
    }

    public override bool checkBoundry(Vector3Int pos)
    {
        return (pos.y < size.y * (1f - height) || pos.z < size.z * (1f - depth));
    }

    public override void setBoundry(int b, float[] x)
    {
        for (int k = 1; k <= size.x; k++)
        {
            for (int i = size.y; i >= size.y * height; i--)
            {
                x[id(k, i, (int)(size.z * (1f - depth)))] = b == 3 ? -x[id(k, i, (int)(size.z * (1f - depth)) - 1)] : x[id(k, i, (int)(size.z * (1f - depth)) - 1)];
                x[id(k, i, (int)(size.z * (1f - depth)) + 1)] = 0;
            }

            for (int i = size.z; i >= size.z * depth; i--)
            {
                x[id(k, (int)(size.y * (1f - height)), i)] = b == 2 ? -x[id(k, (int)(size.y * (1f - height)) - 1, i)] : x[id(k, (int)(size.y * (1f - height)) - 1, i)];
                x[id(k, (int)(size.y * (1f - height)) + 1, i)] = 0;
            }
        }
    }
}

class ObstacleBack : Obstacle
{
    public ObstacleBack(float height, float depth, Vector3Int size) : base(height, depth, size)
    {
        obstaclePos = new Vector3Int(size.x / 2, size.y - ((int)(size.y * height) / 2), (int)(size.z * depth) / 2);
        obstacleSize = new Vector3Int(size.x, (int)(size.y * height), (int)(size.z * depth));
    }

    public override bool checkBoundry(Vector3Int pos)
    {
        return (pos.y < size.y * (1f - height) || pos.z > size.z * depth);
    }

    public override void setBoundry(int b, float[] x)
    {
        for (int k = 1; k <= size.x; k++)
        {
            for (int i = size.y; i >= size.y * height; i--)
            {
                x[id(k, i, (int)(size.z * depth))] = b == 3 ? -x[id(k, i, (int)(size.z * depth) + 1)] : x[id(k, i, (int)(size.z * depth) + 1)];
                x[id(k, i, (int)(size.z * depth) - 1)] = 0;
            }

            for (int i = 1; i <= size.z * depth; i++)
            {
                x[id(k, (int)(size.y * (1f - height)), i)] = b == 2 ? -x[id(k, (int)(size.y * (1f - height)) - 1, i)] : x[id(k, (int)(size.y * (1f - height)) - 1, i)];
                x[id(k, (int)(size.y * (1f - height)) + 1, i)] = 0;
            }
        }
    }
}

class ObstacleRight : Obstacle
{
    public ObstacleRight(float height, float depth, Vector3Int size) : base(height, depth, size)
    {
        obstaclePos = new Vector3Int(size.x - ((int)(size.x * depth) / 2), size.y - ((int)(size.y * height) / 2), size.z / 2);
        obstacleSize = new Vector3Int((int)(size.x * depth), (int)(size.y * height), size.z);
    }

    public override bool checkBoundry(Vector3Int pos)
    {
        return (pos.y < size.y * (1f - height) || pos.x < size.x * (1f - depth));
    }

    public override void setBoundry(int b, float[] x)
    {
        for (int k = 1; k <= size.z; k++)
        {
            for (int i = size.y; i >= size.y * height; i--)
            {
                x[id((int)(size.x * (1f - depth)), i, k)] = b == 1 ? -x[id((int)(size.x * (1f - depth)) - 1, i, k)] : x[id((int)(size.x * (1f - depth)) - 1, i, k)];
                x[id((int)(size.x * (1f - depth)) + 1, i, k)] = 0;
            }

            for (int i = size.x; i >= size.x * depth; i--)
            {
                x[id(i, (int)(size.y * (1f - height)), k)] = b == 2 ? -x[id(i, (int)(size.y * (1f - height)) - 1, k)] : x[id(i, (int)(size.y * (1f - height)) - 1, k)];
                x[id(i, (int)(size.y * (1f - height)) + 1, k)] = 0;
            }
        }
    }
}

class ObstacleLeft : Obstacle
{
    public ObstacleLeft(float height, float depth, Vector3Int size) : base(height, depth, size)
    {
        obstaclePos = new Vector3Int((int)(size.x * depth) / 2, size.y - ((int)(size.y * height) / 2), size.z / 2);
        obstacleSize = new Vector3Int((int)(size.x * depth), (int)(size.y * height), size.z);
    }

    public override bool checkBoundry(Vector3Int pos)
    {
        return (pos.y < size.y * (1f - height) || pos.x > size.x * depth);
    }

    public override void setBoundry(int b, float[] x)
    {
        for (int k = 1; k <= size.z; k++)
        {
            for (int i = size.y; i >= size.y * height; i--)
            {
                x[id((int)(size.x * depth), i, k)] = b == 1 ? -x[id((int)(size.x * depth) + 1, i, k)] : x[id((int)(size.x * depth) + 1, i, k)];
                x[id((int)(size.x * depth) - 1, i, k)] = 0;
            }

            for (int i = 1; i <= size.x * depth; i++)
            {
                x[id(i, (int)(size.y * (1f - height)), k)] = b == 2 ? -x[id(i, (int)(size.y * (1f - height)) - 1, k)] : x[id(i, (int)(size.y * (1f - height)) - 1, k)];
                x[id(i, (int)(size.y * (1f - height)) + 1, k)] = 0;
            }
        }
    }
}

public class RectangleObstacle : MonoBehaviour {

    public SmokeManager smokeManager;
    public bool enable = true;
    public enum HorizontalSide { front, back, left, right};
    public HorizontalSide side;
    [Range(0f,1f)]
    public float height = 0.4f;
    [Range(0f,1f)]
    public float depth = 0.2f;

    private Obstacle obstacle;

    void OnDrawGizmosSelected()
    {
        if (!enable || smokeManager == null) return;
        CreateObstacle();
        Vector3 size = new Vector3(obstacle.obstacleSize.x / smokeManager.gridScale + 1,
                                   obstacle.obstacleSize.y / smokeManager.gridScale + 1,
                                   obstacle.obstacleSize.z / smokeManager.gridScale + 1);
        Gizmos.DrawWireCube(smokeManager.gridToWorldPos(obstacle.obstaclePos, true), size);
    }

    private void Awake()
    {
        CreateObstacle();
        smokeManager.fluidBoundries.AddBoundriesCondition(checkBoundry);
        smokeManager.fluidBoundries.AddBoundriesSetter(setBoundry);
    }

    private void CreateObstacle()
    {
        Vector3Int size = smokeManager.size - new Vector3Int(2, 2, 2);
        switch (side)
        {
            case HorizontalSide.front:
                obstacle = new ObstacleFront(height, depth, size);
                break;
            case HorizontalSide.back:
                obstacle = new ObstacleBack(height, depth, size);
                break;
            case HorizontalSide.left:
                obstacle = new ObstacleLeft(height, depth, size);
                break;
            case HorizontalSide.right:
                obstacle = new ObstacleRight(height, depth, size);
                break;
        }
    }

    public bool checkBoundry(int x, int y, int z)
    {
        if (!enable) return true;
        return obstacle.checkBoundry(new Vector3Int(x,y,z));
    }

    public void setBoundry(int b, float[] x)
    {
        if (!enable) return;

        obstacle.setBoundry(b, x);
    }
}
