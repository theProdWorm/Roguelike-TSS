using UnityEngine;

public class State_PlayerDeath : StateMachineBehaviour
{
    private static readonly int DEAD = Animator.StringToHash("dead");

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(DEAD, false);
    }
}
