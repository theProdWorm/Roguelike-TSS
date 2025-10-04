using System.Collections.Generic;
using UnityEngine;

namespace Actors.Enemies.DarkFairy
{
    public class State_DarkFairy_Shield : StateMachineBehaviour
    {
        private static readonly int IDLE = Animator.StringToHash("idle");
        
        [SerializeField] private Entity _minionPrefab;
        
        [SerializeField] private float _shieldAmount = 30f;

        [SerializeField] private float _spawnRange = 3f;
        [SerializeField] private float _spawnDelay = 1f;
        [SerializeField] private int _maxMinions;

        private DarkFairy _darkFairy;

        private Animator _animator;

        private float _remainingShield;

        private float _spawnTimer;
        private readonly List<Entity> _aliveMinions = new();
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;
            
            _darkFairy = animator.GetComponent<DarkFairy>();
            _darkFairy.Invincible = true;
            
            _remainingShield = _shieldAmount;

            _darkFairy.OnDamageTaken.AddListener(OnDamageTaken);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0)
                SpawnMinion();
        }

        private void SpawnMinion()
        {
            if (_aliveMinions.Count >= _maxMinions)
                return;
            
            _spawnTimer = _spawnDelay;
            
            var pos = Random.insideUnitCircle * _spawnRange;
            var minionInstance = Instantiate(_minionPrefab, pos, Quaternion.identity);
            minionInstance.OnDeath.AddListener(minion => _aliveMinions.Remove(minion));
            
            _aliveMinions.Add(minionInstance);
        }

        private void OnDamageTaken(float damage)
        {
            _remainingShield -= damage;

            if (_remainingShield > 0)
                return;
                
            _darkFairy.Invincible = false;
            _animator.SetBool(IDLE, true);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _darkFairy.ResetShieldCooldown();
            _darkFairy.OnDamageTaken.RemoveListener(OnDamageTaken);
        }
    }
}