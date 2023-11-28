using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPrefab : MonoBehaviour
{
    public GameObject WallPrefab;
    public GameObject Plane;

    public void SpawnWalls(Cell cell)
    {
        cell.plane = Plane;
        if (cell.HasWall(Wall.DOWN)) { Instantiate(WallPrefab, transform.position, Quaternion.LookRotation(new Vector3(0,0,-1)), transform); }
        if (cell.HasWall(Wall.UP)) { Instantiate(WallPrefab, transform.position, Quaternion.LookRotation(new Vector3(0,0,1)), transform); }
        if (cell.HasWall(Wall.LEFT)) { Instantiate(WallPrefab, transform.position, Quaternion.LookRotation(new Vector3(-1,0,0)), transform); }
        if (cell.HasWall(Wall.RIGHT))  { Instantiate(WallPrefab, transform.position, Quaternion.LookRotation(new Vector3(1,0,0)), transform); }
    }
}
