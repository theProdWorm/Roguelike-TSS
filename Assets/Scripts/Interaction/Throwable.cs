using UnityEngine;

namespace Interaction
{
    public class Throwable : MonoBehaviour
    {
        private Vector3 _velocity;
        
        public virtual void Throw(Vector2 dir, float initialVelocity)
        {
            _velocity = initialVelocity * dir;
        }

        protected virtual void FixedUpdate()
        {
            transform.position += _velocity * Time.fixedDeltaTime;
            _velocity *= 0.95f;
        }
    }
}