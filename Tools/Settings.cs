using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings 
{
    #region Animator Type
    public static int isRun = Animator.StringToHash("isRun");
    public static int isIdle = Animator.StringToHash("isIdle");
    #endregion

    #region AstarParameter
    public const float rebuildCoolTime = 5f;
    public static List<Vector3Int> nineWay = new List<Vector3Int>()
    {
        new Vector3Int(0, 0, 0),
        new Vector3Int(1, 0, 0), 
        new Vector3Int(0, 1, 0), 
        new Vector3Int(-1, 0, 0), 
        new Vector3Int(0, -1, 0), 
        new Vector3Int(1, 1, 0), 
        new Vector3Int(1, -1, 0), 
        new Vector3Int(-1, 1, 0),
        new Vector3Int(-1, -1, 0)
    };
    #endregion
}
