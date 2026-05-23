using Pathfinding;
using UnityEngine;

namespace Actors.Enemies.DarkFairy
{
    public class State_DarkFairy_Idle : StateMachineBehaviour
    {
        private static readonly int IDLE = Animator.StringToHash("idle");
        
        private Transform _targetPlayer;
        private DarkFairy _darkFairy;
        private Rigidbody2D _rigidbody;

        private NavigationArea _navArea;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Pick a random player to target
            var players = FindObjectsByType<Player>(FindObjectsSortMode.None);
            int index = Random.Range(0, players.Length);
        
            _targetPlayer = players[index].transform;

            _darkFairy = animator.GetComponent<DarkFairy>();
            _rigidbody = animator.GetComponent<Rigidbody2D>();
            
            _navArea = FindFirstObjectByType<NavigationArea>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var playerDistanceVector = _targetPlayer.position - animator.transform.position;
            if (playerDistanceVector.magnitude <= 1)
            {
                _rigidbody.linearVelocity = Vector2.zero;
                return;
            }
            
            bool canPathToPlayer = Pathfinder.FindPath(_navArea, animator.transform.position, _targetPlayer.position, out var path);
            var nextNode = path[1];

            var moveVector = nextNode.Position - (Vector2) animator.transform.position;
            
            var movementVector = _darkFairy.MoveSpeed * moveVector.normalized;
            _rigidbody.linearVelocity = movementVector;
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(IDLE, false);
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }
}