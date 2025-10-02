using UnityEngine;

namespace Actors.Enemies.DarkFairy
{
    public class State_DarkFairy_Teleport : StateMachineBehaviour
    {
        [SerializeField] private float _maxDistance;

        private DarkFairy _darkFairy;
        
        private Transform _teleportTarget;
        private Vector2 _teleportPosition;
        private bool _positionLocked;
    
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var players = FindObjectsByType<Player>(FindObjectsSortMode.None);
            int index = Random.Range(0, players.Length);
        
            _teleportTarget = players[index].transform;

            _darkFairy = animator.GetComponent<DarkFairy>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_positionLocked || stateInfo.normalizedTime < 0.8f)
                return;

            _teleportPosition = Vector2.MoveTowards(
                animator.transform.position,
                _teleportTarget.position, _maxDistance);
            
            _positionLocked = true;
        }
    
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.transform.position = _teleportPosition;
            
            _positionLocked = false;
            _darkFairy.ResetTeleportCooldown();
        }
    }
}
