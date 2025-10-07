using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using Weapons;
using Weapons.Attacks;

namespace Actors.Enemies.Bat
{
    public class State_Bat_Shoot : StateMachineBehaviour
    {
        private static readonly int SWITCH_STATE = Animator.StringToHash("switchState");

        [SerializeField] private float _duration;

        private float _stateTimer;

        private Weapon _weapon;
        
        private Transform _target;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateTimer = _duration;
            
            var players = FindObjectsByType<Player>(FindObjectsSortMode.None);
            int index = Random.Range(0, players.Length);
            
            _target = players[index].transform;
            
            var entity = animator.GetComponent<Entity>();
            _weapon = animator.GetComponentInChildren<Weapon>();
            _weapon.SetEquipper(entity);
            _weapon.transform.right = (_target.position - animator.transform.position).normalized;

            _weapon.Attacking = true;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateTimer -= Time.deltaTime;
            
            Vector3 pos = animator.transform.position;
            var toPlayer = _target.position - pos;
            
            _weapon.transform.right = toPlayer.normalized;
            
            if (_stateTimer <= 0)
                animator.SetTrigger(SWITCH_STATE);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _weapon.Attacking = false;
        }
    }
}