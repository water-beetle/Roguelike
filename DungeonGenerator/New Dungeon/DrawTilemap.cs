using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class DrawTilemap : MonoBehaviour
{
    public Tilemap floorTilemap, wallTilemap, wallTopTilemap, wallTopRightTilemap, AstarTilemap;
    public Tile floorTile;
    public Tile testTile, enemyTargetTile;
    public Tile wallMid, wallTopMid, wallTopRight, wallTopLeft, wallSideMidLeft, wallSideMidRight;
    public Tile wallCornerRight, wallCornerTopRight, wallCornerLeft, wallCornerTopLeft, 
        wallSideFrontLeft, wallSideFrontRight, wallSideTopLeft, wallSideTopRight,
        wallInnerCornerTopRight, wallInnerCornerTopLeft, wallInnerCornerMidLeft, wallInnerCornerMidRight,
        wallCornerBottomRight, wallCornerBottomLeft, wallCornerFrontRight, wallCornerFrontLeft, wallBannerBlue;

    public void DrawFloorTile(HashSet<Vector3Int> totalFloorPositions)
    {
        DrawTile(totalFloorPositions, floorTilemap, floorTile);
    }

    public void DrawWallTile(HashSet<Vector3Int> totalWallPositions, HashSet<Vector3Int> totalFloorPositions)
    {
        foreach(var wallPos in totalWallPositions)
        {
            String type = WallGenerator.MakeWallType(totalFloorPositions, wallPos);
            DrawBasicWalls(wallPos, type);
        }
    }

    public void DrawTile(HashSet<Vector3Int> floorPositions, Tilemap tilemap, Tile tile)
    {
        foreach(var position in floorPositions)
        {
            tilemap.SetTile(position, tile);
        }
    }

    public void DrawSingleTile(Vector3Int position, Tilemap tilemap, Tile tile)
    {
        tilemap.SetTile(position, tile);
    }

    public void ClearTile(Tilemap tilemap)
    {
        tilemap.ClearAllTiles();
    }

    public void ClearAllTilemap()
    {
        ClearTile(floorTilemap);
        ClearTile(wallTilemap);
        ClearTile(wallTopTilemap);
        ClearTile(wallTopRightTilemap);

    }

    public void DrawBasicWalls(Vector3Int basicWallPos, string type)
    {
        int typeInt = Convert.ToInt32(type, 2);
        Tile tile = null;
        Tile tile2 = null;
        Tile tile3 = null;
        Tilemap tilemap = wallTilemap;
        Tilemap topTilemap = wallTopTilemap;

        HashSet<Vector3Int> newFloorTiles = new HashSet<Vector3Int>();

        if (WallByteTypes.wallSideMidLeft.Contains(typeInt))
        {
            tile = wallSideMidLeft;
        }
        else if (WallByteTypes.wallSideMidRight.Contains(typeInt))
        {
            tile = wallSideMidRight;
        }
        else if (WallByteTypes.wallTop.Contains(typeInt))
        {
            tile = wallMid;
            tile2 = wallTopMid;
        }
        else if (WallByteTypes.wallBottom.Contains(typeInt))
        {
            tile = wallMid;
            tile2 = wallTopMid;
        }
        else if (WallByteTypes.wallCornerTopLeft.Contains(typeInt))
        {
            tile = wallCornerLeft;
            tile2 = wallCornerTopLeft;
        }
        else if (WallByteTypes.wallCornerTopRight.Contains(typeInt))
        {
            tile = wallCornerRight;
            tile2 = wallCornerTopRight;
            topTilemap = wallTopRightTilemap;
        }
        else if (WallByteTypes.wallCornerBottomRight.Contains(typeInt))
        {
            tile = wallCornerFrontRight;
            tile2 = wallCornerBottomRight;
            tile3 = wallSideTopLeft;
            topTilemap = wallTopRightTilemap;
        }
        else if (WallByteTypes.wallCornerBottomLeft.Contains(typeInt))
        {
            tile = wallCornerFrontLeft;
            tile2 = wallCornerBottomLeft;
            tile3 = wallSideTopRight;
        }
        else if (WallByteTypes.wallSideFrontLeft.Contains(typeInt))
        {
            tile = wallSideFrontLeft;
        }
        else if (WallByteTypes.wallSideFrontRight.Contains(typeInt))
        {
            tile = wallSideFrontRight;
        }
        else if (WallByteTypes.wallSideTopRight.Contains(typeInt))
        {
            tile = wallSideMidRight;
            tile2 = wallSideTopRight;
            topTilemap = wallTopRightTilemap;
        }
        else if (WallByteTypes.wallSideTopLeft.Contains(typeInt))
        {
            tile = wallSideMidLeft;
            tile2 = wallSideTopLeft;
        }
        else if (WallByteTypes.wallDownBlank.Contains(typeInt))
        {
            tile = wallCornerLeft;
            tile2 = wallCornerTopLeft;
            DrawSingleTile(basicWallPos, wallTopTilemap, wallSideMidLeft);
        }
        else if (WallByteTypes.wallUpBlank.Contains(typeInt))
        {
            tile = wallCornerFrontLeft;
            tile2 = wallCornerBottomLeft;
            DrawSingleTile(basicWallPos + new Vector3Int(0, 1, 0), wallTopRightTilemap, wallSideMidLeft);
        }
        else if (WallByteTypes.wallLeftBlank.Contains(typeInt))
        {
            tile = wallCornerRight;
            tile2 = wallCornerTopRight;
            //DrawSingleTile(basicWallPos, wallTopTilemap, wallTopMid);
        }
        else if (WallByteTypes.wallRightBlank.Contains(typeInt))
        {
            tile = wallCornerLeft;
            tile2 = wallCornerTopLeft;
            //DrawSingleTile(basicWallPos, wallTopTilemap, wallTopMid);
        }
        else if (WallByteTypes.wallUpDown.Contains(typeInt))
        {
            tile = wallMid;
            tile2 = wallTopMid;
            DrawSingleTile(basicWallPos, wallTopTilemap, wallTopMid);
        }
        else if (WallByteTypes.wallLeftRight.Contains(typeInt))
        {
            tile = wallMid;
            DrawSingleTile(basicWallPos, wallTopTilemap, wallSideMidLeft);
            DrawSingleTile(basicWallPos, wallTopRightTilemap, wallSideMidRight);
        }
        else if (WallByteTypes.makeFloor.Contains(typeInt)){

        }

        DrawSingleTile(basicWallPos, wallTilemap, tile);
        if(tile2 != null)
            DrawSingleTile(basicWallPos + new Vector3Int(0, 1, 0), topTilemap, tile2);
        if(tile3 != null)
            DrawSingleTile(basicWallPos + new Vector3Int(0, 2, 0), floorTilemap, tile3);

    }
}
