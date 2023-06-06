using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEngine;

public static class WallGenerator 
{
    public static string MakeWallType(HashSet<Vector3Int> floorPositions, Vector3Int wallPos)
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


    public static HashSet<Vector3Int> GetWallPositions(HashSet<Vector3Int> floorPositions, List<Vector3Int> wayList)
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
