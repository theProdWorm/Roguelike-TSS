using System.Collections.Generic;
using System.Linq;
using Actors;
using UnityEngine;

namespace Weapons.Attacks
{
    // To be used with animation events
    public class AnimatedAttack : Attack
    {
        private bool _active;
        
        private readonly List<Collider2D> _collisions = new();
        
        // Call from animation event
        public void Activate()
        {
            var entities = _collisions.Select(c => c.GetComponent<Entity>());
            
            foreach (var entity in entities)
            {
                entity.TakeDamage(_damage);
            }
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(_enemyTag))
                return;
            
            _collisions.Add(other);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(_enemyTag))
                return;
            
            _collisions.Remove(other);
        }
    }
}