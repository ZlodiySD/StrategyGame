using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid: MonoBehaviour
{
    private int
        width,
        height;
    private GameObject[,] gridArray;

    public Grid(int width, int height)
    {
        this.width = width;
        this.height = height;

        gridArray = new GameObject[width, height];
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y);
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        //x = Mathf.FloorToInt((worldPosition).x / cellSize);
        //y = Mathf.FloorToInt((worldPosition).y / cellSize);

        x = Mathf.RoundToInt(worldPosition.x);
        y = Mathf.RoundToInt(worldPosition.y);
    }

    public GameObject SetValue(int x, int y, GameObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
            gridArray[x, y] = value;
        //Debug.Log("Grid element added by cord x: " + x + " y: " + y);
        return value;
    }

    public GameObject SetValue(Vector3 worldPosition, GameObject value)
    {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);

        return value;
    }

    public GameObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
            return gridArray[x, y];
        else
            return null;
    }

    public GameObject GetValue(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetValue(x, y);
    }

    public void MoveTo(int targetX, int targetY, GameObject value)
    {
        GetXY(value.transform.position, out int x, out int y);
        SetValue(targetX, targetY, value);
        value.transform.position = new Vector3(targetX, targetY, value.transform.position.z);
        SetValue(x, y, null);
    }

    public void MoveTo(Vector3 worldPosition, GameObject value)
    {
        GetXY(worldPosition, out int x, out int y);
        MoveTo(x, y, value);
    }

    public void RemoveValue(int x, int y)
    {
        if(GetValue(x, y).gameObject != null)
            Destroy(GetValue(x, y).gameObject);
        SetValue(x, y, null);
    }

    public void RemoveValue(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        RemoveValue(x, y);
    }
}
