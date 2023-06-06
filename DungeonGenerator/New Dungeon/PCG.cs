using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PCG 
{

    public static HashSet<Vector3Int> RandomWalkIteration(int iteration, int length, Vector3Int startPosition, BoundsInt roomRect, int offset)
    {
        HashSet<Vector3Int> randomwalkResultPosition = new HashSet<Vector3Int>();
        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

        for (int i = 0; i < iteration; i++)
        {
            randomwalkResultPosition = RandomWalk(length, startPosition, roomRect, offset);
            floorPositions.UnionWith(randomwalkResultPosition);
            startPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }

        return floorPositions;
    }

    public static HashSet<Vector3Int> RandomWalk(int length, Vector3Int startPosition, BoundsInt roomRect, int offset)
    {
        Vector3Int currentPosition = startPosition;

        HashSet<Vector3Int> totalPositions = new HashSet<Vector3Int>();

        for (int i = 0; i < length; i++)
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

        while (rectQueue.Count > 0)
        {
            BoundsInt rect = rectQueue.Dequeue();

            int length = rect.size.x > rect.size.y ? rect.size.x : rect.size.y;
            length = Mathf.RoundToInt(Random.Range(0.4f, 0.7f) * length);

            if (rect.size.x >= minWidth && rect.size.y >= minHeight)
            {
                if (rect.size.x >= rect.size.y && rect.size.x > minWidth * 2)
                {
                    BoundsInt divideRect1 = new BoundsInt(rect.min, new Vector3Int(length, rect.size.y, rect.size.z));
                    BoundsInt divideRect2 = new BoundsInt(new Vector3Int(rect.min.x + length, rect.min.y, rect.min.z),
                        new Vector3Int(rect.size.x - length, rect.size.y, rect.size.z));
                    rectQueue.Enqueue(divideRect1);
                    rectQueue.Enqueue(divideRect2);
                }
                else if (rect.size.y > rect.size.x && rect.size.y > minHeight * 2)
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
