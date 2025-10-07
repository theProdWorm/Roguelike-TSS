using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;

    private void Awake()
    {
        float speed = Random.Range(_minSpeed, _maxSpeed);
        if (Random.value > 0.5f)
            speed *= -1;
        _rigidbody.angularVelocity = speed;
        
    }
}
