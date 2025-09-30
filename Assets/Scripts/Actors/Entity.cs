using UnityEngine;
using UnityEngine.Events;

namespace Actors
{
    public class Entity : MonoBehaviour
    {
        public UnityEvent OnDamageTaken;
        
        [Header("Stats")]
        [SerializeField] protected float _maxHealth = 100f;
        [SerializeField] protected float _moveSpeed = 3f;

        [Header("References")]
        [SerializeField] protected Rigidbody2D _rigidbody;

        protected float _health;
        
        private void Awake()
        {
            _health = _maxHealth;
        }
        
        public virtual void TakeDamage(float damage)
        {
            _health -= damage;
            
            OnDamageTaken.Invoke();
        }
        
        private void DestroySelf() => Destroy(gameObject);
    }
}
