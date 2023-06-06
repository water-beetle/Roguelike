using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;

public static class Astar
{
    public static Stack<Vector3Int> BuildPath(Vector3Int startGridPosition, Vector3Int endGridPosition, BoundsInt roomBox)
    {
        // 입력받은 start, end gridPosition은 월드 좌표 기준이다. roomBox.min을 빼주어
        // 원점을 해당하는 방의 왼쪽 하단으로 변경한다.
        startGridPosition -= roomBox.min; 
        endGridPosition -= roomBox.min;

        List<Node> openNodeList = new List<Node>();
        HashSet<Node> closedNodeHashSet = new HashSet<Node>();

        GridNodes gridNodes = new GridNodes(roomBox.size.x, roomBox.size.y);

        Node startNode = gridNodes.GetGridNode(startGridPosition.x, startGridPosition.y);
        Node targetNode = gridNodes.GetGridNode(endGridPosition.x, endGridPosition.y);

        Node endPathNode = FindShortestPath(startNode, targetNode, gridNodes, openNodeList, closedNodeHashSet, roomBox);

        if(endPathNode != null)
        {
            return CreatePathStack(endPathNode, roomBox);
        }

        return new Stack<Vector3Int>();
    }

    private static Node FindShortestPath(Node startNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, BoundsInt roomBox)
    {
        openNodeList.Add(startNode);

        while(openNodeList.Count > 0)
        {
            openNodeList.Sort();

            Node currentNode = openNodeList[0];
            openNodeList.RemoveAt(0);
            closedNodeHashSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                return currentNode;
            }

            EvaluateCurrentNodeNeighbours(currentNode, targetNode, gridNodes, openNodeList, closedNodeHashSet, roomBox);
        }

        return null;
    }

    private static void EvaluateCurrentNodeNeighbours(Node currentNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, BoundsInt roomBox)
    {
        Vector3Int currentNodeGridPosition = currentNode.gridPosition;

        Node validNeighbourNode;

        for(int i=-1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0 || Math.Abs(i) == Math.Abs(j))
                    continue;

                validNeighbourNode = GetValidNodeNeighbour(currentNodeGridPosition.x + i, currentNodeGridPosition.y + j, gridNodes, closedNodeHashSet, roomBox);
            
                if(validNeighbourNode != null)
                {
                    int newCostToNeighbour;

                    newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, validNeighbourNode);

                    bool isValidNeighbourNodeInOpenList = openNodeList.Contains(validNeighbourNode);

                    if(newCostToNeighbour < validNeighbourNode.gCost || !isValidNeighbourNodeInOpenList)
                    {
                        validNeighbourNode.gCost = newCostToNeighbour;
                        validNeighbourNode.hCost = GetDistance(validNeighbourNode, targetNode);
                        validNeighbourNode.parentNode = currentNode;

                        if (!isValidNeighbourNodeInOpenList)
                            openNodeList.Add(validNeighbourNode);
                    }

                }
            }
        }
    }

    private static int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int dstY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    private static Node GetValidNodeNeighbour(int neighbourNodeXPosition, int neighbourNodeYPosition, GridNodes gridNodes, HashSet<Node> closedNodeHashSet, BoundsInt roomBox)
    {
        if(neighbourNodeXPosition >= roomBox.size.x || neighbourNodeXPosition < 0 || neighbourNodeYPosition >= roomBox.size.y || neighbourNodeYPosition < 0)
        {
            return null;
        }

        Node neighbourNode = gridNodes.GetGridNode(neighbourNodeXPosition, neighbourNodeYPosition);

        if (closedNodeHashSet.Contains(neighbourNode) || DungeonManager.Instance.obstaclePosition.Contains(neighbourNode.gridPosition))
        {
            return null;
        }
        else
        {
            return neighbourNode;
        }

    }

    private static Stack<Vector3Int> CreatePathStack(Node targetNode, BoundsInt roomBox)
    {
        Stack<Vector3Int> movementPathStack = new Stack<Vector3Int>();

        Node nextNode = targetNode;
        while(nextNode != null)
        {
            Vector3Int pos = nextNode.gridPosition;
            movementPathStack.Push(pos);
            nextNode = nextNode.parentNode;
        }
        return movementPathStack;

    }
}

