using UnityEngine;
using UnityEngine.Splines;

namespace Weapons.Attacks
{
    public class ProjectileAttack : Attack
    {
        [SerializeField] private Spline _path;
        [SerializeField] private float _speed;

        private float _distanceTraveled;
        private float _totalPathLength;
        
        private Vector3 _startPosition;

        public void Initialize(Spline path, float speed, float damage, float lifeTime)
        {
            Initialize(damage, lifeTime);
            
            _path.Copy(path);
            _speed = speed;
            
            _totalPathLength = path.CalculateLength(transform.worldToLocalMatrix);
            
            _startPosition = transform.position;
        }
        
        private void FixedUpdate()
        {
            _distanceTraveled += _speed * Time.fixedDeltaTime;
            
            float t = Mathf.Clamp01(_distanceTraveled / _totalPathLength);
            
            Vector3 pos = _startPosition + (Vector3) _path.EvaluatePosition(t);
            transform.position = pos;
            
            if (Mathf.Approximately(t, 1f))
                Destroy(gameObject);
        }

        protected override void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (!otherCollider.CompareTag("Enemy"))
                return;
            
            base.OnTriggerEnter2D(otherCollider);
        }
    }
}