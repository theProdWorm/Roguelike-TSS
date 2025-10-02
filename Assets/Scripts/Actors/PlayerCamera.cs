using System.Collections;
using UnityEngine;

namespace Actors
{
    public class PlayerCamera : MonoBehaviour
    {
        public Transform Target;
        [SerializeField] public Vector3 Offset;
    
        [Header("Screen Shake")]
    
        [InspectorName("General Intensity")]
        [Range(0, 1)]
        [SerializeField] private float _screenShakeIntensityGeneral = 1f;

        private float _lastSetIntensity;
        
        private Camera _camera;
    
        private Vector2 _shakeOffset;
    
        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            _camera.transform.position = Target.position + Offset + (Vector3) _shakeOffset;
        }

        public void SetShakeIntensity(float intensity)
        {
            _lastSetIntensity = intensity;
        }
        
        public void Shake(float duration)
        {
            float outputIntensity = _lastSetIntensity * _screenShakeIntensityGeneral;
            
            StartCoroutine(ShakeRoutine(outputIntensity, duration));
        }

        private IEnumerator ShakeRoutine(float intensity, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
            
                float angle = Random.Range(0f, 2 * Mathf.PI);
                float distance = Random.Range(0f, intensity);
            
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            
                _shakeOffset = distance * dir;

                yield return null;
            }
        }
    }
}
