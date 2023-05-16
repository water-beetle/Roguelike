using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public static class CorridorGenerator
{
    public static HashSet<Vector3Int> Corridors(List<BoundsInt> rooms)
    {
        Kruskal kruskal = new Kruskal();
        List<TwoPos> twoPositions = kruskal.GetSortedTwoPositions(rooms.Count, rooms);
        
        
        return GetCorridorPos(twoPositions);
    }

    public static HashSet<Vector3Int> GetCorridorPos(List<TwoPos> twoPositions)
    {
        HashSet<Vector3Int> corridors = new HashSet<Vector3Int>();

        foreach(var pos in twoPositions)
        {
            corridors.UnionWith(Connect(pos));
        }

        return corridors;
    }

    public static HashSet<Vector3Int> Connect(TwoPos twoPos)
    {
        Vector3Int source = twoPos.a;
        Vector3Int destination = twoPos.b;

        HashSet<Vector3Int> corridors = new HashSet<Vector3Int>();

        while(source.x != destination.x)
        {
            if (source.x > destination.x)
                source.x -= 1;
            else
                source.x += 1;

            corridors.Add(source);
        }

        while(source.y != destination.y)
        {
            if (source.y > destination.y)
                source.y -= 1;
            else
                source.y += 1;

            corridors.Add(source);
        }

        return corridors;
    }
}

public class Kruskal
{
    List<Edge> edges = new List<Edge>();
    List<TwoPos> twoPosList;
    int[] unionCheck;

    class Edge
    {
        public Vector3Int a, b;
        public int indA, indB;
        public float distance;

        public Edge(Vector3 a, Vector3 b, int i, int j)
        {
            this.a = new Vector3Int(Mathf.RoundToInt(a.x), Mathf.RoundToInt(a.y), Mathf.RoundToInt(a.z));
            this.b = new Vector3Int(Mathf.RoundToInt(b.x), Mathf.RoundToInt(b.y), Mathf.RoundToInt(b.z));
            this.distance = Vector3.Distance(a, b);
            this.indA = i;
            this.indB = j;
        }
    }

    void init(int numberOfRooms, List<BoundsInt> rooms)
    {
        twoPosList = new List<TwoPos>();
        unionCheck = new int[numberOfRooms];

        for (int i = 0; i < numberOfRooms; i++)
        {
            for (int j = i + 1; j < numberOfRooms; j++)
            {
                Edge newEdge = new Edge(rooms[i].center, rooms[j].center, i, j);
                edges.Add(newEdge);
            }
            unionCheck[i] = i;
        }

        edges = edges.OrderBy(x => x.distance).ToList();
    }

    int Root(int a)
    {
        while (a != unionCheck[a])
            a = unionCheck[a];

        return a;
    }

    bool isAllUnion()
    {
        int first = Root(unionCheck[0]);

        foreach(var item in unionCheck)
        {
            if (Root(item) != first)
                return false;
        }

        return true;
    }

    void Union(int a, int b) // 큰값을 부모로 Union
    {
        a = Root(a);
        b = Root(b);

        unionCheck[a] = b;
    }

    public List<TwoPos> GetSortedTwoPositions(int numberOfRooms, List<BoundsInt> rooms)
    {
        List<TwoPos> corridorPairs = new List<TwoPos>();
        init(numberOfRooms, rooms);

        foreach (var edge in edges)
        {
            corridorPairs.Add(new TwoPos(edge.a, edge.b));
            Union(edge.indA, edge.indB);

            if (isAllUnion())
                break;

        }

        return corridorPairs;
    }

}

public struct TwoPos
{
    public Vector3Int a;
    public Vector3Int b;

    public TwoPos(Vector3Int a, Vector3Int b)
    {
        this.a = a;
        this.b = b;
    }
}

