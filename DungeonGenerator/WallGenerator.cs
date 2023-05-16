using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEngine;

public static class WallGenerator 
{
    public static void CreateWalls(HashSet<Vector3Int> floorPositions, DrawTilemap painter)
    {
        HashSet<Vector3Int> basicWalls = GetWallPositions(floorPositions, RandomPosition.eightWay);

        CreateBasicWall(basicWalls, floorPositions, painter);
    }

    private static void CreateBasicWall(HashSet<Vector3Int> basicWalls, HashSet<Vector3Int> floorPositions, DrawTilemap painter)
    {
        foreach (var basicWallPos in basicWalls)
        {
            String type = MakeWallType(floorPositions, basicWallPos);
            painter.DrawBasicWalls(basicWallPos, type);


        }
    }

    private static string MakeWallType(HashSet<Vector3Int> floorPositions, Vector3Int wallPos)
    {
        string type = "";
        foreach (var direction in RandomPosition.eightWay)
        {
            var checkPos = wallPos + direction;

            if (floorPositions.Contains(checkPos))
            {
                type += "1";
            }
            else
            {
                type += "0";
            }
        }

        return type;
    }


    private static HashSet<Vector3Int> GetWallPositions(HashSet<Vector3Int> floorPositions, List<Vector3Int> wayList)
    {
        HashSet<Vector3Int> wallPositions = new HashSet<Vector3Int>();
        foreach(var position in floorPositions)
        {
            foreach(var direction in wayList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition) == false)
                    wallPositions.Add(neighbourPosition);
            }
        }
        return wallPositions;
    }


}
