using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    public DrawTilemap painter;
    [Space(10)]
    [Header("RandomWalk Parameters")]
    public int iteration;
    public int length;
    [Space(10)]
    [Header ("BinaryPartitioning Parameters")]
    public int offset = 1;
    public int width, height;
    public int minWidth, minHeight;

    HashSet<Vector3Int> floorPositions, wallPositions, corridorPositions;
    List<BoundsInt> roomRects;

    public void GenerateDungeon()
    {
        painter.ClearTile(painter.floorTilemap);
        painter.ClearTile(painter.wallTilemap);
        painter.ClearTile(painter.wallTopTilemap);
        painter.ClearTile(painter.wallTopRightTilemap);

        roomRects = GetRoomsRects(new Vector3Int(0, 0, 0), width, height, minWidth, minHeight);
        floorPositions = RandomWalkGenerate(roomRects, iteration, length);
        corridorPositions = CorridorGenerator.Corridors(roomRects);
        floorPositions.UnionWith(corridorPositions);

        WallGenerator.CreateWalls(floorPositions, painter);

        painter.DrawTile(floorPositions, painter.floorTilemap, painter.floorTile);
    }

    private HashSet<Vector3Int> RandomWalkIteration(int iteration, int length, Vector3Int startPosition, BoundsInt roomRect)
    {
        HashSet<Vector3Int> randomwalkResultPosition = new HashSet<Vector3Int>();
        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

        for (int i = 0; i < iteration; i++)
        {
            randomwalkResultPosition = PCGAlgorithm.RandomWalk(length, startPosition, roomRect, offset);
            floorPositions.UnionWith(randomwalkResultPosition);
            startPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }

        return floorPositions;
    }

    private List<BoundsInt> GetRoomsRects(Vector3Int startPosition, int width, int height, int minWidth, int minHeight)
    {
        List<Vector3Int> roomPositions = new List<Vector3Int>();
        BoundsInt dungeonRect = new BoundsInt(startPosition, new Vector3Int(width, height, 0));
        List<BoundsInt> roomRects;

        roomRects = PCGAlgorithm.BinarySpacePartitioning(minWidth, minHeight, dungeonRect);


        return roomRects;
    }

    private HashSet<Vector3Int> RandomWalkGenerate(List<BoundsInt> roomRects, int iteration, int length)
    {
        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

        foreach(var roomRect in roomRects)
        {
            Vector3Int roomCenter = new Vector3Int(Mathf.RoundToInt(roomRect.center.x), Mathf.RoundToInt(roomRect.center.y), Mathf.RoundToInt(roomRect.center.z));
            HashSet<Vector3Int> randomwalk = RandomWalkIteration(iteration, length, roomCenter, roomRect);
            floorPositions.UnionWith(randomwalk);
        }

        return floorPositions;
    }
}
