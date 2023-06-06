using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region EnemyStatus
    private float targetDistance = 5f;
    private float health;
    private Vector2 dir;
    public float speed;
    #endregion

    public GameObject target;

    public Rigidbody2D rgbd;
    public Animator animator;

    
    // Start is called before the first frame update
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        animator= GetComponent<Animator>();
        animator.SetBool(Settings.isRun, true);
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Vector2.Distance(target.transform.position, transform.position) < targetDistance)
        {
            animator.SetBool(TypeHelper.isRun, true);
        }
        else
        {
            animator.SetBool(TypeHelper.isRun, false);
        }*/
    }

    private void OnDrawGizmos()
    {
     
        //Stack<Vector3> posStack = Astar.BuildPath(transform.position, target.transform.position, );
    }
}
