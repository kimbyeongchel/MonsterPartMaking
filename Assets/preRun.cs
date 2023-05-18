using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class preRun : StateMachineBehaviour
{
    Enemy enemy;
    Transform enemyTransform;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        enemyTransform = animator.GetComponent<Transform>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(enemyTransform.position, enemy.target.position) > 5f)
        {
            animator.SetBool("isFollow", false);
        }
        else if (Vector2.Distance(enemyTransform.position, enemy.target.position) > 2f)
            enemyTransform.position = Vector2.MoveTowards(enemyTransform.position, enemy.target.position, Time.deltaTime * enemy.moveSpeed);
        else
        {
            animator.SetBool("isFollow", false);
        }

        enemy.DirectionEnemy(enemy.target.position.x, enemyTransform.position.x);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
