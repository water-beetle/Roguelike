using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition 
{
  
    public static List<Vector3Int> fourWay = new List<Vector3Int>()
    {
        new Vector3Int(0, 1, 0), //up
        new Vector3Int(1, 0, 0), //right
        new Vector3Int(0, -1, 0), //down
        new Vector3Int(-1, 0, 0), //left
    };

    public static List<Vector3Int> eightWay = new List<Vector3Int>()
    {
        new Vector3Int(0, 1, 0), //up
        new Vector3Int(1, 1, 0), // upright
        new Vector3Int(1, 0, 0), //right
        new Vector3Int(1, -1, 0), // downright
        new Vector3Int(0, -1, 0), //down
        new Vector3Int(-1, -1, 0), // downleft
        new Vector3Int(-1, 0, 0), //left
        new Vector3Int(-1, 1, 0) // upleft
    };



    public static Vector3Int getRandomPosition()
    {
        int pick = Random.Range(0, fourWay.Count);
        return fourWay[pick];
    }
    
}
