using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Graphics
{
    public class LightManipulator : MonoBehaviour
    {
        [SerializeField] private Light2D _light;
        [SerializeField] private float _lerpTime;

        private void Start()
        {
            if (_light)
                return;
            
            _light = FindObjectsByType<Light2D>(FindObjectsSortMode.None)[0];
        }
        
        public void SetIntensity(float intensity)
        {
            _light.intensity = intensity;
        }

        private void LerpIntensity(float intensity)
        {
            StartCoroutine(LerpIntensityRoutine(intensity));
        }

        public void ResetIntensity(float delay)
        {
            StartCoroutine(SetIntensityWithDelay(delay, 1f));
        }

        private IEnumerator SetIntensityWithDelay(float delay, float intensity)
        {
            yield return new WaitForSeconds(delay);
            
            SetIntensity(intensity);
        }

        private IEnumerator LerpIntensityRoutine(float intensity)
        {
            float elapsedTime = 0f;
            float startIntensity = _light.intensity;
            
            while (elapsedTime < _lerpTime)
            {
                elapsedTime += Time.deltaTime;
            
                float value = Mathf.Lerp(startIntensity, intensity, elapsedTime / _lerpTime);
                SetIntensity(value);
                
                yield return null;
            }
        }
    }
}
