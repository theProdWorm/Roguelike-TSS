using UnityEngine;
using UnityEngine.Events;

namespace Actors
{
    public abstract class Entity : MonoBehaviour
    {
        public UnityEvent<Entity> OnDeath;
        public UnityEvent OnDamageTaken;
        
        [Header("Stats")]
        public float MaxHealth = 100f;
        public float MoveSpeed = 3f;

        [Header("References")]
        [SerializeField] protected Rigidbody2D _rigidbody;

        protected float _health;
        
        protected virtual void Awake()
        {
            _health = MaxHealth;
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
