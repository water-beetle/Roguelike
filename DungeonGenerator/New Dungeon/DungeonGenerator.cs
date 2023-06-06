using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DungeonGenerator
{

    public static List<BoundsInt> GetDungeonRoomRects(Vector3Int startPosition, int width, int height, int minWidth, int minHeight)
    {
        List<Vector3Int> roomPositions = new List<Vector3Int>();
        List<BoundsInt> roomRects;
        BoundsInt dungeonRect = new BoundsInt(startPosition, new Vector3Int(width, height, 0));

        roomRects = PCG.BinarySpacePartitioning(minWidth, minHeight, dungeonRect);

        return roomRects;
    }

    public static List<HashSet<Vector3Int>> GetDungeonRoomFloorPositions(List<BoundsInt> roomRects, int iteration, int length, int offset)
    {
        List<HashSet<Vector3Int>> roomFloorPositions = new List<HashSet<Vector3Int>>();

        foreach (var roomRect in roomRects)
        {
            Vector3Int roomCenter = new Vector3Int(Mathf.RoundToInt(roomRect.center.x), Mathf.RoundToInt(roomRect.center.y), Mathf.RoundToInt(roomRect.center.z));
            HashSet<Vector3Int> randomwalk = PCG.RandomWalkIteration(iteration, length, roomCenter, roomRect, offset);
            roomFloorPositions.Add(randomwalk);
        }

        return roomFloorPositions;
    }

    public static HashSet<Vector3Int> GetDungeonCorridorsPositions(List<BoundsInt> roomsRects)
    {
        HashSet<Vector3Int> corridorPositions = CorridorGenerator.Corridors(roomsRects);

        return corridorPositions;
    }

    public static List<HashSet<Vector3Int>> GetDungeonWallsPositions(HashSet<Vector3Int> totalFloorPositions, List<BoundsInt> roomRects)
    {
        HashSet<Vector3Int> wallPositions = WallGenerator.GetWallPositions(totalFloorPositions, RandomPosition.eightWay);

        List<HashSet<Vector3Int>> roomWallPositions = new List<HashSet<Vector3Int>>();
        for (int i = 0; i < roomRects.Count; i++)
        {
            roomWallPositions.Add(new HashSet<Vector3Int>());
        }

        foreach (var pos in wallPositions)
        {
            for (int i = 0; i < roomRects.Count; i++)
            {
                if (isContain(roomRects[i], pos))
                {
                    roomWallPositions[i].Add(pos);
                }
                
            }
        }

        return roomWallPositions;
    }

    // 기존 Unity의 BoundsInt는 xMin, yMin, zMin <= 입력 값 < xMax, yMax, zMax를 만족해야 true를 반환한다.
    // 그런데 사용하는 모든 z 좌표값들은 0이라 zMin == zMax라 BoundsInt.Contain 함수를 제대로 사용할 수 없어 새로운 함수가 필요
    public static bool isContain(BoundsInt rect, Vector3Int position)
    {
        if (rect.xMin <= position.x && position.x < rect.xMax && rect.yMin <= position.y && position.y <= rect.yMax)
            return true;
        else
            return false;
    }

    public static HashSet<Vector3Int> GetDungeonTotalFloorPositions(List<HashSet<Vector3Int>> roomFloorPositions, HashSet<Vector3Int> corridorPositions)
    {
        HashSet<Vector3Int> totalFloorPositions = new HashSet<Vector3Int>();

        foreach(var roomFloorPosition in roomFloorPositions)
        {
            totalFloorPositions.UnionWith(roomFloorPosition);
        }

        totalFloorPositions.UnionWith(corridorPositions);

        return totalFloorPositions;
    }

    public static HashSet<Vector3Int> GetDungeonTotalWallPositions(List<HashSet<Vector3Int>> roomWallPositions)
    {
        HashSet<Vector3Int> totalWallPositions = new HashSet<Vector3Int>();
        foreach(var roomWallPosition in roomWallPositions)
        {
            totalWallPositions.UnionWith(roomWallPosition);
        }

        return totalWallPositions;
    }
}
