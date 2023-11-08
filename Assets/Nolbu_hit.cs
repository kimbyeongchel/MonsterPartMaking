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
            nolbu.hitCount = 0; // idle 상태에서 바로 arrowUP으로 갈때 hit에 남은 숫자가 있으면 화살공격이 1, 2번으로 끝날 수 있기에 초기화 진행
            nolbu.currentPatternCoroutine = null; // 현재 코루틴을 멈췄으니 초기화
            nolbu.RemovePrefabs(); // 화면상의 모든 저장된 프리팹들 삭제
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
