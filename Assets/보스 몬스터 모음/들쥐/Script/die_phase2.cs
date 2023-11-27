using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class die_phase2 : StateMachineBehaviour
{
    Bossmouse mouse;
    float step; // �����Ӵ� �̵� �Ÿ�
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mouse = GameObject.FindGameObjectWithTag("Bossrat").GetComponent<Bossmouse>();
        mouse.render.color = Color.white;
        mouse.pcoll.enabled = false;
        mouse.ccoll.enabled = false;
        mouse.bColl.enabled = false;
        mouse.transform.position += new Vector3(0f, -0.6f, 0f);
        mouse.rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        mouse.render.flipX = true;
        mouse.monsterSpeed = 7f;
        step = mouse.monsterSpeed * Time.deltaTime; // �����Ӵ� �̵� �Ÿ�
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mouse.transform.position = Vector3.MoveTowards(mouse.transform.position, new Vector3(-7.15f, mouse.transform.position.y, 0f), step);

        if (mouse.transform.position.x == new Vector3(-7.15f, mouse.transform.position.y, 0f).x)
        { 
            mouse.monsterDestroy();
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
