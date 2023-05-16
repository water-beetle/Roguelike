using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public static class PCGAlgorithm
{
    public static HashSet<Vector3Int> RandomWalk(int length, Vector3Int startPosition, BoundsInt roomRect, int offset)
    {
        Vector3Int currentPosition = startPosition;

        HashSet<Vector3Int> totalPositions = new HashSet<Vector3Int>();

        for(int i=0; i<length; i++)
        {
            
            if (roomRect.xMin + offset < currentPosition.x && currentPosition.x < roomRect.xMax - offset && 
                roomRect.yMin + offset < currentPosition.y && currentPosition.y < roomRect.yMax - offset)
            {
                totalPositions.Add(currentPosition);
            }
            else
            {
                currentPosition = totalPositions.ElementAt(Random.Range(0, totalPositions.Count));
            }
            currentPosition += RandomPosition.getRandomPosition();
        }
        return totalPositions;
    }

    public static List<BoundsInt> BinarySpacePartitioning(int minWidth, int minHeight, BoundsInt startRect)
    {
        Queue<BoundsInt> rectQueue = new Queue<BoundsInt>();
        List<BoundsInt> rectPositions = new List<BoundsInt>();

        rectQueue.Enqueue(startRect);

        while(rectQueue.Count > 0)
        {
            BoundsInt rect = rectQueue.Dequeue();

            int length = rect.size.x > rect.size.y ? rect.size.x : rect.size.y;
            length = Mathf.RoundToInt(Random.Range(0.4f, 0.7f) * length);
            
            if(rect.size.x >= minWidth && rect.size.y >= minHeight)
            {
                if (rect.size.x >= rect.size.y && rect.size.x > minWidth * 2)
                {
                    BoundsInt divideRect1 = new BoundsInt(rect.min, new Vector3Int(length, rect.size.y, rect.size.z));
                    BoundsInt divideRect2 = new BoundsInt(new Vector3Int(rect.min.x + length, rect.min.y, rect.min.z), 
                        new Vector3Int(rect.size.x - length, rect.size.y, rect.size.z));
                    rectQueue.Enqueue(divideRect1);
                    rectQueue.Enqueue(divideRect2);
                }
                else if(rect.size.y > rect.size.x && rect.size.y > minHeight * 2)
                {
                    BoundsInt divideRect1 = new BoundsInt(rect.min, new Vector3Int(rect.size.x, length, rect.size.z));
                    BoundsInt divideRect2 = new BoundsInt(new Vector3Int(rect.min.x, rect.min.y + length, rect.min.z),
                        new Vector3Int(rect.size.x, rect.size.y - length, rect.size.z));
                    rectQueue.Enqueue(divideRect1);
                    rectQueue.Enqueue(divideRect2);
                }
                else
                {
                    rectPositions.Add(rect);
                }
                    
            }
        }

        return rectPositions;
    }

}

public static class RandomPosition
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
