using UnityEngine;

namespace Interaction
{
    public class PickupFactory : MonoBehaviour
    {
        [SerializeField] private Pickup _pickupPrefab;
    
        public Pickup CreatePickup(Object referenceObject, Vector2 position)
        {
            return Instantiate(_pickupPrefab, position, Quaternion.identity, transform);
        }
    }
}