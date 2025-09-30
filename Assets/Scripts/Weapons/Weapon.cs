using System.Collections;
using UnityEngine;
using Weapons.Attacks;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] protected Attack[] _attackSequence;
        [SerializeField] protected float[] _attackDelays;
        
        [SerializeField] private float _sequenceTimeout = 1f;
        
        [Tooltip("If checked, treats the full attack sequence as \"one\" attack.")]
        public bool Burst;
        
        public float Damage = 1f;

        private SpriteRenderer _spriteRenderer;
        
        public bool Attacking { get; set; }
        private float _attackTimer;
        private int _currentSequenceIndex;
        
        private float _sequenceTimer;

        private Coroutine _attackRoutine;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_attackSequence.Length == 0)
                Debug.LogError($"{name}: No attacks registered.");
            if (_attackSequence.Length != _attackDelays.Length)
                Debug.LogError($"{name}: Attack sequence and attack delays lists do not match.");
        }
        
        private void Update()
        {
            // Decrease timer when attack has finished
            if (_attackRoutine == null && _attackTimer > 0f)
                _attackTimer -= Time.deltaTime;

            // Perform attack as soon as timer reaches zero, if attacking
            if (Attacking && _attackTimer <= 0f)
            {
                if (Burst)
                    _attackRoutine = StartCoroutine(AttackRoutine());
                else
                    PerformAttack();
            }

            // Reset sequence after an idle delay
            if (_sequenceTimer > 0)
                _sequenceTimer -= Time.deltaTime;
            if (_sequenceTimer <= 0)
                _currentSequenceIndex = 0;
        }

        private IEnumerator AttackRoutine()
        {
            int remainingAttacks = _attackSequence.Length;
            while (remainingAttacks > 0)
            {
                // Set delay
                float attackTimer = _attackDelays[_currentSequenceIndex];
                
                PerformAttack();
                
                remainingAttacks--;
                
                // Wait before the next attack in the sequence
                while (attackTimer > 0f)
                {
                    attackTimer -= Time.deltaTime;
                    yield return null;
                }
            }

            _attackRoutine = null;
        }

        protected virtual Attack PerformAttack()
        {
            _attackTimer = _attackDelays[_currentSequenceIndex];
            _sequenceTimer = _sequenceTimeout;
            
            Attack attack = _attackSequence[_currentSequenceIndex];
            Attack attackInstance = Instantiate(attack, transform.position, transform.rotation);
            
            attackInstance.Initialize(Damage, 1f);
            
            _currentSequenceIndex++;
            _currentSequenceIndex %= _attackSequence.Length; // Loop sequence

            return attackInstance;
        }

        public void FlipSprite(bool flip)
        {
            _spriteRenderer.flipY = flip;
        }
    }
}