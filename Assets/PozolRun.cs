using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PozolRun : StateMachineBehaviour
{
    Enemy enemy;
    Transform enemyTransform;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        enemyTransform = animator.GetComponent<Transform>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(enemyTransform.position, enemy.target.position) > 6f)
        {
            animator.SetBool("isFollow", false);
        }
        else if (Vector2.Distance(enemyTransform.position, enemy.target.position) > 2f)
        {
            enemyTransform.position = Vector2.MoveTowards(enemyTransform.position, enemy.target.position, Time.deltaTime * enemy.moveSpeed);
        }
        else
        {
            animator.SetBool("isFollow", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
