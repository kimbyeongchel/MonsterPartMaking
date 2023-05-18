using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ready : StateMachineBehaviour
{
    Enemy enemy;
    Transform enemyTransform;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        enemyTransform = animator.GetComponent<Transform>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(enemy.atkDelay <= 0)
            animator.SetTrigger("attack");

        if (Vector2.Distance(enemyTransform.position, enemy.target.position) > 1f)
        {
            animator.SetBool("isFollow", true);
        }

        enemy.DirectionEnemy(enemy.target.position.x, enemyTransform.position.x);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
