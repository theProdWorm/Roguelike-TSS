using UnityEngine;

namespace Actors.Enemies.DarkFairy
{
    public class State_DarkFairy_Idle : StateMachineBehaviour
    {
        private static readonly int PLAYER_DISTANCE = Animator.StringToHash("playerDistance");
        private static readonly int IDLE = Animator.StringToHash("idle");
        
        private Transform _targetPlayer;
        private DarkFairy _darkFairy;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Pick a random player to target
            var players = FindObjectsByType<Player>(FindObjectsSortMode.None);
            int index = Random.Range(0, players.Length);
        
            _targetPlayer = players[index].transform;

            _darkFairy = animator.GetComponent<DarkFairy>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var dir = (_targetPlayer.position - animator.transform.position).normalized;
            _darkFairy.SetMoveDir(dir);
            
            var distanceToPlayer = Vector2.Distance(animator.transform.position, _targetPlayer.position);
            animator.SetFloat(PLAYER_DISTANCE, distanceToPlayer);
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(IDLE, false);
            _darkFairy.SetMoveDir(Vector2.zero);
        }
    }
}