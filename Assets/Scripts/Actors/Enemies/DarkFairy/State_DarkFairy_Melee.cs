using System.Linq;
using UnityEngine;

namespace Actors.Enemies.DarkFairy
{
    public class State_DarkFairy_Melee : StateMachineBehaviour
    {
        private static readonly int IDLE = Animator.StringToHash("idle");

        private DarkFairy _darkFairy;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _darkFairy = animator.GetComponent<DarkFairy>();
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _darkFairy.ResetMeleeCooldown();
        }
    }
}