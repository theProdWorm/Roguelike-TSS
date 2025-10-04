using Actors;
using UnityEngine;

namespace Weapons.Attacks
{
    public class Attack : MonoBehaviour
    {
        [SerializeField] protected float _damage = 1f;
        [SerializeField] protected float _lifeTime = 1f;

        protected string _enemyTag;
        protected string _allyTag;
        
        public void Initialize(float damage, string allyTag)
        {
            _damage = damage;

            _allyTag = allyTag;
            _enemyTag = allyTag == "Enemy" ? "Player" : "Enemy";
        }

        protected virtual void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (!otherCollider.CompareTag(_enemyTag))
                return;

            var entity = otherCollider.GetComponent<Entity>();
            entity.TakeDamage(_damage);
        }
    }
}