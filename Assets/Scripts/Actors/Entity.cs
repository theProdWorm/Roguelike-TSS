using UnityEngine;
using UnityEngine.Events;

namespace Actors
{
    public abstract class Entity : MonoBehaviour
    {
        public UnityEvent<Entity> OnDeath;
        public UnityEvent<float> OnDamageTaken;
        
        [Header("Stats")]
        public float MaxHealth = 10f;
        public float MoveSpeed = 3f;

        [Header("References")]
        [SerializeField] protected Rigidbody2D _rigidbody;

        protected float _health;
        protected bool _dead;
        
        public bool Invincible { get; set; }
        
        protected virtual void Awake()
        {
            _health = MaxHealth;
        }

        public float GetHealth() => _health;
        
        public Vector2 GetVelocity() => _rigidbody.linearVelocity;
        
        public virtual void TakeDamage(float damage)
        {
            if (!Invincible)
                _health -= damage;

            OnDamageTaken.Invoke(damage);
            
            if (_health <= 0)
                Die();
        }

        protected virtual void Die()
        {
            if (_dead)
                return;
            
            OnDeath.Invoke(this);
            _dead = true;
        }
        
        private void DestroySelf() => Destroy(gameObject);

        // private void OnCollisionEnter2D(Collision2D collision)
        // {
        //     if (!collision.otherCollider.CompareTag("Wall"))
        //         return;
        //     
        //     // Handle wall collisions
        //     foreach (var contact in collision.contacts)
        //     {
        //         Vector2 correction = contact.normal * -contact.separation;
        //         _rigidbody.position += correction;
        //     }
        // }
    }
}
