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
            Vector3 targetPosition = new Vector3(enemy.target.position.x, enemyTransform.position.y, enemyTransform.position.z);
            enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, targetPosition, enemy.moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isFollow", false);
        }

        enemy.DirectionEnemy(enemy.target.position.x, enemyTransform.position.x);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
