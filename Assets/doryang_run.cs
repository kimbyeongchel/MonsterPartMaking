using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doryang_run : StateMachineBehaviour
{
    Bossmouse mouse;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mouse = GameObject.FindGameObjectWithTag("Bossrat").GetComponent<Bossmouse>();
        mouse.Check_distance();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mouse.right == true)
        {
            mouse.rb.AddForce(Vector2.right * mouse.moveSpeed);
        }
        else
        {
            mouse.rb.AddForce(Vector2.left * mouse.moveSpeed);
        }

        mouse.distance_move += Mathf.Abs(mouse.rb.velocity.x) * Time.deltaTime;
        if (mouse.distance_move >= mouse.rayDistance)
        {
            mouse.rb.velocity = Vector2.zero;
            mouse.rb.AddForce(Vector2.zero);
            mouse.distance_move = 0f;
            mouse.bossAni.SetBool("run", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
