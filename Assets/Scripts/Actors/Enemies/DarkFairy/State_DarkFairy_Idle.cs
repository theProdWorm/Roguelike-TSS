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

        private CircleCollider2D _playerCollider;
        private CircleCollider2D _darkFairyCollider;
        
        private NavigationArea _navArea;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Pick a random player to target
            var players = FindObjectsByType<Player>(FindObjectsSortMode.None);
            int index = Random.Range(0, players.Length);
        
            _targetPlayer = players[index].transform;
            
            _darkFairy = animator.GetComponent<DarkFairy>();
            _rigidbody = animator.GetComponent<Rigidbody2D>();
            
            _playerCollider = players[index].GetComponent<CircleCollider2D>();
            _darkFairyCollider = animator.GetComponent<CircleCollider2D>();
            
            _navArea = FindFirstObjectByType<NavigationArea>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _rigidbody.linearVelocity = Vector2.zero;
            
            var playerDistanceVector = _targetPlayer.position - animator.transform.position;
            if (playerDistanceVector.magnitude <= 1)
                return;
            
            var playerPos = (Vector2) _targetPlayer.position + _playerCollider.offset;
            var darkFairyPos = (Vector2) _darkFairy.transform.position + _darkFairyCollider.offset;
            
            bool canPathToPlayer = Pathfinder.FindPath(_navArea, darkFairyPos, playerPos, out var path);
            if (!canPathToPlayer)
            {
                Debug.Log("couldn't find a path");
                return;
            }
            
            var nextNode = path[path.Count <= 2 ? 1 : 2];
            var moveVector = nextNode.Position - darkFairyPos;
            
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