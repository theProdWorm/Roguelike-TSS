using UnityEngine;

namespace Interaction
{
    public class Throwable : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        
        public virtual void Throw(Vector2 dir, float initialVelocity)
        {
            _rigidbody.linearVelocity = initialVelocity * dir;
        }

        protected virtual void FixedUpdate()
        {
            _rigidbody.linearVelocity *= 0.83f;
        }
    }
}