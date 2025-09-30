using Actors;
using UnityEngine;

namespace Weapons.Attacks
{
    public class Attack : MonoBehaviour
    {
        [SerializeField] protected float _damage = 1f;
        [SerializeField] protected float _lifeTime = 1f;

        private float _timeLived;
        
        public void Initialize(float damage, float lifeTime)
        {
            _damage = damage;
            _lifeTime = lifeTime;
        }

        private void Update()
        {
            _timeLived += Time.deltaTime;
            
            if (_timeLived >= _lifeTime)
                Destroy(gameObject);
        }

        protected virtual void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (!otherCollider.CompareTag("Enemy"))
                return;

            var entity = otherCollider.GetComponent<Entity>();
            entity.TakeDamage(_damage);
        }
    }
}