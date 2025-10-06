using Actors;
using UnityEngine;

namespace Interaction
{
    public class Pickup : Throwable, IInteractable
    {
        [SerializeField] private Collider2D _collider;

        public void Interact(Player player)
        {
            player.PickUp(gameObject);

            Throw(Vector2.zero, 0);
            
            tag = "Untagged";
        }

        public override void Throw(Vector2 dir, float initialVelocity)
        {
            base.Throw(dir, initialVelocity);

            tag = "Interactable";
        }
    }
}