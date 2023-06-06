using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : Singleton<DungeonManager>
{
    public HashSet<Vector3Int> totalFloorPositions, totalWallPositions, corridorPositions, obstaclePosition;
    public List<BoundsInt> roomRects;
    public List<HashSet<Vector3Int>> roomFloorPositions, roomWallPositions;

    public DrawTilemap painter;
    public GridLayout gridLayout;

    [Space(10)]
    [Header("RandomWalk Parameters")]
    public int iteration;
    public int length;
    [Space(10)]
    [Header("BinaryPartitioning Parameters")]
    public int offset = 1;
    public int width, height;
    public int minWidth, minHeight;

    public void GenerateDungeon()
    {
        ClearAllTile();

        // 바닥 타일 위치 정보(일반 방, 복도)
        roomRects = DungeonGenerator.GetDungeonRoomRects(new Vector3Int(0, 0, 0), width, height, minWidth, minHeight);
        roomFloorPositions = DungeonGenerator.GetDungeonRoomFloorPositions(roomRects, iteration, length, offset);
        corridorPositions = DungeonGenerator.GetDungeonCorridorsPositions(roomRects);
        // 각각의 Room의 Floor와 Corridor을 하나의 HashSet으로 합친 결과
        totalFloorPositions = DungeonGenerator.GetDungeonTotalFloorPositions(roomFloorPositions, corridorPositions);

        // 벽 타일 위치 정보
        roomWallPositions = DungeonGenerator.GetDungeonWallsPositions(totalFloorPositions, roomRects);
        totalWallPositions = DungeonGenerator.GetDungeonTotalWallPositions(roomWallPositions);
        obstaclePosition.UnionWith(totalWallPositions);

        DrawTile(totalFloorPositions, totalWallPositions);
    }

    public BoundsInt DungeonRect
    {
        get
        {
            return new BoundsInt(new Vector3Int(0, 0, 0), new Vector3Int(width, height, 0));
        }
    }

    private void ClearAllTile()
    {
        painter.ClearAllTilemap();
    }

    public void DrawTile(HashSet<Vector3Int> totalFloorPositions, HashSet<Vector3Int> totalWallPositions)
    {
        painter.DrawFloorTile(totalFloorPositions);
        painter.DrawWallTile(totalWallPositions, totalFloorPositions);

    }

    public override void Awake()
    {
        base.Awake();
        obstaclePosition = new HashSet<Vector3Int>();
        gridLayout = GetComponentInChildren<GridLayout>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ClearAllTile();
    }

    // Update is called once per frame
    void Update()
    {
        painter.ClearTile(painter.AstarTilemap);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach(var room in roomRects)
            Gizmos.DrawWireCube(room.center, room.size);
    }

}
