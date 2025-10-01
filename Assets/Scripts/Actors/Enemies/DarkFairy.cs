using UnityEngine;

namespace Actors.Enemies
{
    public class DarkFairy : Enemy
    {
        private static readonly int PLAYER_DISTANCE = Animator.StringToHash("playerDistance");
        private static readonly int SPELL_READY = Animator.StringToHash("spellReady");
        private static readonly int TELEPORT_READY = Animator.StringToHash("teleportReady");
        private static readonly int SHIELD_READY = Animator.StringToHash("shieldReady");
        
        [SerializeField] private float _teleportCooldown = 5f;
        [SerializeField] private float _spellCooldown = 5f;
        [SerializeField] private float _shieldCooldown = 5f;
        [SerializeField] private float _spellDamage = 2f;

        private float _teleportTimer;
        private float _spellTimer;
        private float _shieldTimer;
        
        private void Update()
        {
            if (_teleportTimer > 0)
            {
                _teleportTimer -= Time.deltaTime;
                bool teleportReady = _teleportTimer <= 0;
                
                _animator.SetBool(TELEPORT_READY, teleportReady);
            }

            if (_spellTimer > 0)
            {
                _spellTimer -= Time.deltaTime;
                bool spellReady = _spellTimer <= 0;
                
                _animator.SetBool(SPELL_READY, spellReady);
            }

            if (_shieldTimer > 0)
            {
                _shieldTimer -= Time.deltaTime;
                bool shieldReady = _shieldTimer <= 0;
                
                _animator.SetBool(SHIELD_READY, shieldReady);
            }
        }
    }
}