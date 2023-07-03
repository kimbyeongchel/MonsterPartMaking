using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    Enemy enemy;
    Transform enemyTransform;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        enemyTransform = animator.GetComponent<Transform>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float dist = Vector2.Distance(enemyTransform.position, enemy.target.position);
        if (dist <= 10f && dist >3f)
            animator.SetBool("isFollow", true);
        else if (enemy.atkDelay <= 0 && Vector2.Distance(enemyTransform.position, enemy.target.position) <= 3f)
        {
            if (enemy.rand.NextDouble() > 0.6)
            {
                animator.SetTrigger("attack1");
            }
            else
                animator.SetTrigger("attack2");

            enemy.atkDelay = enemy.atkCooltime;
        }

        enemy.DirectionEnemy(enemy.target.position.x, enemyTransform.position.x);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
