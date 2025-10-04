using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemies.Bat
{
    public class State_Bat_Move : StateMachineBehaviour
    {
        private static readonly int SWITCH_STATE = Animator.StringToHash("switchState");

        [SerializeField] private float _targetDistanceFromPlayer;

        private float _moveTimer;
        
        private Rigidbody2D _rigidbody;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var entity = animator.GetComponent<Entity>();
            _rigidbody = animator.GetComponent<Rigidbody2D>();
            
            var player = FindObjectsByType<Player>(FindObjectsSortMode.None)[0];
            
            float rotAngle = Random.Range(0f, 360f);
            var rot = Quaternion.AngleAxis(rotAngle, Vector3.forward);

            var targetPos = player.transform.position + _targetDistanceFromPlayer * (rot * Vector3.right);
            
            float moveDistance = Vector2.Distance(targetPos, animator.transform.position);
            float timeToReach = moveDistance / entity.MoveSpeed;
            _moveTimer = timeToReach;
            
            _rigidbody.linearVelocity = entity.MoveSpeed * (targetPos - animator.transform.position).normalized;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _moveTimer -= Time.deltaTime;
            
            if (_moveTimer <= 0)
                animator.SetTrigger(SWITCH_STATE);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }
}