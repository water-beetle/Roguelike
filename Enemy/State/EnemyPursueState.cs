using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Apple;

public class EnemyPursueState : StateMachineBehaviour
{
    private EnemyController _enemyController;
    private DrawTilemap painter;

    private Stack<Vector3Int> pathStack;
    private float rebuildCoolTime;
    private Coroutine moveEnemy;

    private float enemyMoveSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemyController = animator.transform.GetComponent<EnemyController>();
        painter = DungeonManager.Instance.GetComponent<DrawTilemap>();
        enemyMoveSpeed = _enemyController.speed;
        rebuildCoolTime = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Pursue();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    private void Pursue()
    {
        rebuildCoolTime -= Time.deltaTime;

        if (rebuildCoolTime <= 0)
        {
            rebuildCoolTime = Settings.rebuildCoolTime;
            pathStack = FindPath();
        }

        MoveEnemy();

        foreach(var pos in pathStack)
        {
            painter.DrawSingleTile(pos, painter.AstarTilemap, painter.testTile);
        }
    }

    private void MoveEnemy()
    {
        if(pathStack.Count != 0)
        {
            Vector3 destination = pathStack.Peek() ;
            Vector3 myPos = _enemyController.transform.position;

            if (Vector3.Distance(destination, myPos) <= 0.1f)
            {
                pathStack.Pop();
            }

            Vector2 moveDir = (destination - myPos).normalized;
            Debug.Log(destination + ", " + myPos);
            _enemyController.rgbd.velocity = (moveDir * enemyMoveSpeed);
        }
    }

    private Stack<Vector3Int> FindPath()
    {
        Vector3Int targetPos = FindNearestValidPosition(_enemyController.target.transform.position);
        Vector3Int myPos = FindNearestValidPosition(_enemyController.transform.position);

        BoundsInt dungeonRect = DungeonManager.Instance.DungeonRect;
        return Astar.BuildPath(myPos, targetPos, dungeonRect);
    }

    private Vector3Int FindNearestValidPosition(Vector3 position)
    {
        // 
        int xPos = Mathf.RoundToInt(position.x);
        int yPos = Mathf.RoundToInt(position.y);
        int zPos = Mathf.RoundToInt(position.z);

        foreach(var pos in Settings.nineWay)
        {
            Vector3Int nearPos = new Vector3Int(xPos, yPos, zPos) + pos;
            if (!DungeonManager.Instance.obstaclePosition.Contains(nearPos))
                return nearPos;
        }
                
        Debug.Log("Player Position Error");
        return new Vector3Int(-1, -1, zPos);
    }
}
