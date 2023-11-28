using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Cell
{
    public Vector2Int gridPosition;
    public Wall walls; //bit Encoded
    public GameObject plane;
    public Color BaseColor;

    public void RemoveWall(Wall wallToRemove)
    {
        walls = (walls & ~wallToRemove);
    }

    public int GetNumWalls()
    {
        int numWalls = 0;
        if ((walls & Wall.DOWN) != 0) { numWalls++; }
        if ((walls & Wall.UP) != 0) { numWalls++; }
        if ((walls & Wall.LEFT) != 0) { numWalls++; }
        if ((walls & Wall.RIGHT) != 0) { numWalls++; }
        return numWalls;
    }

    public bool HasWall(Wall wallDirection)
    {
        return (walls & wallDirection) != 0;
    }

    public List<Cell> GetNeighbours(Cell[,] grid)
    {
        List<Cell> result = new List<Cell>();
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                // Skip if the cell is the current cell or diagonal (based on your code's logic)
                if (Mathf.Abs(x) == Mathf.Abs(y))
                    continue;

                int cellX = this.gridPosition.x + x;
                int cellY = this.gridPosition.y + y;

                // Check grid bounds
                if (cellX < 0 || cellX >= grid.GetLength(0) || cellY < 0 || cellY >= grid.GetLength(1))
                    continue;

                // Check for walls
                if ((x == -1 && HasWall(Wall.LEFT)) || (x == 1 && HasWall(Wall.RIGHT)) ||
                    (y == -1 && HasWall(Wall.DOWN)) || (y == 1 && HasWall(Wall.UP)))
                    continue;

                result.Add(grid[cellX, cellY]);
            }
        }
        return result;
    }
    public void Highlight()
    {
        MeshRenderer renderer = plane.GetComponent<MeshRenderer>();
        BaseColor = renderer.material.color;
        renderer.material.color = new Color(0, 1, 0);
    }

    public void DeHighlight()
    {
        MeshRenderer renderer = plane.GetComponent<MeshRenderer>();
        if (BaseColor != null)
        {
            renderer.material.color = BaseColor;
        }
    }
}


[System.Flags]
public enum Wall
{
    LEFT = 1,
    UP = 2,
    RIGHT = 4,
    DOWN = 8
}
