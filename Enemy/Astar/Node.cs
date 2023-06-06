using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node>
{
    public Vector3Int gridPosition;
    public int gCost = 0; // distance from starting node
    public int hCost = 0; // estimated distance to destination
    public Node parentNode;

    public Node(Vector3Int gridPosition)
    {
        this.gridPosition = gridPosition;
        parentNode = null;
    }

    public int FCost
    {
        get { return gCost + hCost; }
    }


    // Node 정렬 기준
    // FCost가 작은 값이 제일 먼저임
    // FCost가 같을 시 GCost가 작은 값이 먼저임
    public int CompareTo(Node other)
    {
        int compare = FCost.CompareTo(other.FCost);

        if(compare == 0)
            compare = hCost.CompareTo(other.hCost);

        return compare;
    }
}
