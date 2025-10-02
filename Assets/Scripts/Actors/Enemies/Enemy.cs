using UnityEngine;
using UnityEngine.Events;

namespace Actors.Enemies
{
    public class Enemy : Entity
    {
        private static readonly int HEALTH = Animator.StringToHash("healthPercent");
        
        [SerializeField] protected Animator _animator;
        
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            
            _animator.SetFloat(HEALTH, _health / _maxHealth);
        }
    }
}