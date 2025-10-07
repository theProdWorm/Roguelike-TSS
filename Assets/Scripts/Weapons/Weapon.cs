using System.Collections;
using Actors;
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

        protected string _allyTag;

        private SpriteRenderer _spriteRenderer;

        public bool Attacking { get; set; }
        private float _attackTimer;
        private int _currentSequenceIndex;

        private float _sequenceTimer;

        private Coroutine _attackRoutine;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            // Decrease timer when attack has finished
            if (_attackTimer > 0f)
                _attackTimer -= Time.deltaTime;
            
            // Perform attack as soon as timer reaches zero, if attacking
            if (Attacking && _attackRoutine == null && _attackTimer < float.Epsilon)
            {
                if (Burst)
                    _attackRoutine = StartCoroutine(AttackRoutine());
                else
                    PerformAttack();
            }

            // Reset sequence after an idle delay
            if (!(_sequenceTimer > 0))
                return;

            _sequenceTimer -= Time.deltaTime;

            if (_sequenceTimer <= float.Epsilon)
                _currentSequenceIndex = 0;
        }

        public void SetEquipper(Entity entity)
        {
            if (entity)
            {
                _allyTag = entity.tag;
                transform.parent = entity.transform;
            }
            else
            {
                _allyTag = "Untagged";
                transform.parent = null;
            }
        }

        private IEnumerator AttackRoutine()
        {
            int remainingAttacks = _attackSequence.Length;
            while (remainingAttacks > 0)
            {
                PerformAttack();

                remainingAttacks--;
                yield return new WaitUntil(() => _attackTimer <= float.Epsilon);
            }

            _attackRoutine = null;
        }

        protected virtual Attack PerformAttack()
        {
            _attackTimer = _attackDelays[_currentSequenceIndex];
            _sequenceTimer = _sequenceTimeout;

            Attack attack = _attackSequence[_currentSequenceIndex];
            Attack attackInstance = Instantiate(attack, transform.position, transform.rotation);

            attackInstance.Initialize(Damage, _allyTag);

            _currentSequenceIndex++;
            _currentSequenceIndex %= _attackSequence.Length; // Loop sequence

            return attackInstance;
        }

        public void FlipSprite(bool flip)
        {
            _spriteRenderer.flipY = flip;
        }

        public void SetVisible(bool visible)
        {
            _spriteRenderer.enabled = visible;
        }

        public Color GetSpriteColor() => _spriteRenderer.color;

        private void OnDisable()
        {
            if (_attackRoutine != null)
            {
                StopCoroutine(_attackRoutine);
                _attackRoutine = null;
            }
        }
    }
}