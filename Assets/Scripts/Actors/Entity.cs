using UnityEngine;
using UnityEngine.Events;

namespace Actors
{
    public abstract class Entity : MonoBehaviour
    {
        public UnityEvent<Entity> OnDeath;
        public UnityEvent OnDamageTaken;
        
        [Header("Stats")]
        [SerializeField] protected float _maxHealth = 100f;
        [SerializeField] protected float _moveSpeed = 3f;

        [Header("References")]
        [SerializeField] protected Rigidbody2D _rigidbody;

        protected float _health;
        
        protected virtual void Awake()
        {
            _health = _maxHealth;
        }
        
        public virtual void TakeDamage(float damage)
        {
            _health -= damage;

            OnDamageTaken.Invoke();
            
            if (_health <= 0)
                Die();
        }

        protected virtual void Die()
        {
            OnDeath.Invoke(this);
        }
        
        private void DestroySelf() => Destroy(gameObject);
    }
}
