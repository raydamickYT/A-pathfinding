using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Astar
{
    Vector2Int gridSize;
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path from the startPos to the endPos
    /// Note that you will probably need to add some helper functions
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    /// 
    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        gridSize = new(grid.GetLength(0), grid.GetLength(1));
        //als dit false is dan heb je buiten de grid geklikt
        if (!CheckIfGrid(endPos, gridSize))
        {
            Debug.LogWarning("hahahah stupid");
            return default;
        }
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        //add start node to open list
        Node startNode = new Node(startPos, null, 0, GetHeuristic(startPos, endPos));
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList.OrderBy(n => n.FScore).First();

            if (currentNode.position == endPos)
            {
                return ConstructPath(currentNode);
            }

            //process neighbours update list
            foreach (Cell neighborCell in findCellWithoutWall(grid, grid[currentNode.position.x, currentNode.position.y]))
            {
                if (closedList.Any(n => n.position == neighborCell.gridPosition))
                    continue; // Skip if the neighbor is in the closed list

                float tentativeGScore = currentNode.GScore + Vector2Int.Distance(currentNode.position, neighborCell.gridPosition);
                Node neighborNode = openList.FirstOrDefault(n => n.position == neighborCell.gridPosition);

                if (neighborNode == null)
                {
                    neighborNode = new Node(neighborCell.gridPosition, currentNode, tentativeGScore, GetHeuristic(neighborCell.gridPosition, endPos));
                    openList.Add(neighborNode);
                }
                else if (tentativeGScore < neighborNode.GScore)
                {
                    neighborNode.GScore = tentativeGScore;
                    neighborNode.parent = currentNode;
                }
            }

            // NOTE: dit was voor visualisatie
            // foreach (var cell in openList)
            // {
            //     grid[cell.position.x, cell.position.y].Highlight();
            // }
            // foreach (var cell in closedList)
            // {
            //     grid[cell.position.x, cell.position.y].DeHighlight();
            // }

            openList.Remove(currentNode);
            closedList.Add(currentNode);
        }
        return null;
    }

    private bool CheckIfGrid(Vector2Int endPos, Vector2Int gridsize)
    {
        bool result;
        Vector2Int i = new(0, 0);
        result = endPos.x < gridSize.x && endPos.y < gridsize.y && endPos.x >= i.x && endPos.y >= i.y;
        return result;
    }
    //vind nogsteeds neighbours
    private List<Cell> findCellWithoutWall(Cell[,] grid, Cell currentCell)
    {
        List<Cell> result = new List<Cell>();

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (Math.Abs(x) == Math.Abs(y))
                    continue;

                int cellX = currentCell.gridPosition.x + x;
                int cellY = currentCell.gridPosition.y + y;

                //dit is dubbel op want de maze spawnt met muren, maar waarom ook niet.
                if (cellX < 0 || cellX >= grid.GetLength(0) || cellY < 0 || cellY >= grid.GetLength(1) || Mathf.Abs(x) == Mathf.Abs(y))
                {
                    continue;
                }
                // Check for walls, if wall in that dir: skip
                if ((x == -1 && currentCell.HasWall(Wall.LEFT)) || (x == 1 && currentCell.HasWall(Wall.RIGHT)) ||
                    (y == -1 && currentCell.HasWall(Wall.DOWN)) || (y == 1 && currentCell.HasWall(Wall.UP)))
                    continue;

                result.Add(grid[cellX, cellY]);
            }
        }


        return result;
    }

    private int GetHeuristic(Vector2Int pointA, Vector2Int pointB)
    {
        int xDistance = Math.Abs(pointA.x - pointB.x);
        int yDistance = Math.Abs(pointA.y - pointB.y);
        return xDistance + yDistance;
    }

    private List<Vector2Int> ConstructPath(Node endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = endNode;
        while (currentNode != null)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public float FScore
        { //GScore + HScore
            get { return GScore + HScore; }
        }
        public float GScore; //Current Travelled Distance
        public float HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, float GScore, int HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}
