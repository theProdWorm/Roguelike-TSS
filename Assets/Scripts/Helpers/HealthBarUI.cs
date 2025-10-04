using Actors;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Helpers
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private Entity _target;
        [SerializeField] private Slider _healthBarSlider;
        [SerializeField] private Image _healthBarImage;

        [SerializeField] private Gradient _gradient;
        
        private void Update()
        {
            var healthPercent = _target.GetHealth() / _target.MaxHealth;
            
            _healthBarSlider.value = healthPercent;
            _healthBarImage.color = _gradient.Evaluate(healthPercent);
        }
    }
}