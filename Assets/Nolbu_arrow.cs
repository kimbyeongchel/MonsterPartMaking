using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nolbu_arrow : StateMachineBehaviour
{
    NewNolbu nolbu;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nolbu = GameObject.FindGameObjectWithTag("NewNolbu").GetComponent<NewNolbu>();
        nolbu.hitCount++;
        nolbu.currentPatternCoroutine = nolbu.StartCoroutine(nolbu.RangeAll());
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (nolbu.hitCount == 3)
        {
            nolbu.bossAni.SetBool("arrowRe", false);
            nolbu.currentPatternCoroutine = null;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (nolbu.hitCount == 3)
        {
            nolbu.hitCount = 0;
        }
    }
}
