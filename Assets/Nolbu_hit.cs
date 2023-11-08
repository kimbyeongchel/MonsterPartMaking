using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nolbu_hit : StateMachineBehaviour
{
    NewNolbu nolbu;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nolbu = GameObject.FindGameObjectWithTag("NewNolbu").GetComponent<NewNolbu>();
        if (nolbu.currentPatternCoroutine != null)
        {
            nolbu.StopCoroutine(nolbu.currentPatternCoroutine);
            nolbu.hitCount = 0; // idle ���¿��� �ٷ� arrowUP���� ���� hit�� ���� ���ڰ� ������ ȭ������� 1, 2������ ���� �� �ֱ⿡ �ʱ�ȭ ����
            nolbu.currentPatternCoroutine = null; // ���� �ڷ�ƾ�� �������� �ʱ�ȭ
            nolbu.RemovePrefabs(); // ȭ����� ��� ����� �����յ� ����
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
